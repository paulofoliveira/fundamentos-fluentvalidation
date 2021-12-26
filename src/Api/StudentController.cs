using DomainModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// [ApiController] Habilita features adicionais como a validação automática (isto é, verifica automaticamente ModelState.IsValid), sem necesidade de fazer em cada Action, não 
// precisa do [FromBody] para o input de dados pois é o padrão
// ControllerBase é uma classe que suporta métodos de API sem View

namespace Api
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Where(x => x.Value.Errors.Any())
            //        .Select(x => x.Value.Errors.First().ErrorMessage).ToArray();

            //    return BadRequest(errors);
            //}

            var addresses = request.Addresses
                .Select(address => new Address(address.Street, address.City, address.State, address.ZipCode))
                .ToArray();

            var email = Email.Create(request.Email);
            var studentName = StudentName.Create(request.Name);

            //if (email.IsFailure)
            //    return BadRequest(email.Error);

            //if (studentName.IsFailure)
            //    return BadRequest(studentName.Error);

            // Não há mais necessidade de validar diretamente, pois o FluentValidation já aplica a validação com a criação de um Value Object correspondente
            // conforme configuração com MustBeValueObject.

            var student = new Student(email.Value, studentName.Value, addresses); ;

            _studentRepository.Save(student);

            var response = new RegisterResponse
            {
                Id = student.Id
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] EditPersonalInfoRequest request)
        {
            //var validator = new EditPersonalInfoRequestValidator();

            //var result = validator.Validate(request);

            //if (!result.IsValid)
            //    return BadRequest(result.Errors[0].ErrorMessage);

            var student = _studentRepository.GetById(id);

            var addresses = request.Addresses
           .Select(address => new Address(address.Street, address.City, address.State, address.ZipCode))
           .ToArray();

            var studentName = StudentName.Create(request.Name);

            if (studentName.IsFailure)
                return BadRequest(studentName.Error);

            student.EditPersonalInfo(studentName.Value, addresses);
            _studentRepository.Save(student);

            return Ok();
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] EnrollRequest request)
        {
            Student student = _studentRepository.GetById(id);

            foreach (CourseEnrollmentDto enrollmentDto in request.Enrollments)
            {
                Course course = _courseRepository.GetByName(enrollmentDto.Course);
                var grade = Enum.Parse<Grade>(enrollmentDto.Grade);

                student.Enroll(course, grade);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Student student = _studentRepository.GetById(id);

            var response = new GetResonse
            {
                Addresses = student.Addresses.Select(address => new AddressDto()
                {
                    Street = address.Street,
                    City = address.City,
                    ZipCode = address.ZipCode,
                    State = address.State
                }).ToArray(),
                Email = student.Email.Value,
                Name = student.Name.Value,
                Enrollments = student.Enrollments.Select(x => new CourseEnrollmentDto
                {
                    Course = x.Course.Name,
                    Grade = x.Grade.ToString()
                }).ToArray()
            };
            return Ok(response);
        }
    }
}
