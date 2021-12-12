using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel
{
    public class Student : Entity
    {
        public string Email { get; }
        public string Name { get; private set; }
        public Address Address { get; private set; }

        private readonly List<Enrollment> _enrollments = new();
        public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

        protected Student()
        {
        }

        public Student(string email, string name, Address address)
            : this()
        {
            Email = email;
            EditPersonalInfo(name, address);
        }

        public void EditPersonalInfo(string name, Address address)
        {
            Name = name;
            Address = address;
        }

        public virtual void Enroll(Course course, Grade grade)
        {
            if (_enrollments.Count >= 2)
                throw new Exception("Cannot have more than 2 enrollments");

            if (_enrollments.Any(x => x.Course == course))
                throw new Exception($"Student '{Name}' already enrolled into course '{course.Name}'");

            var enrollment = new Enrollment(this, course, grade);
            _enrollments.Add(enrollment);
        }
    }
}
