using DomainModel;
using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class StudentRepository
    {
        private static readonly List<Student> _existingStudents = new List<Student>
        {
            Alice(),
            Bob()
        };

        private static long _lastId = _existingStudents.Max(x => x.Id);

        public Student GetById(long id)
        {
            return _existingStudents.SingleOrDefault(x => x.Id == id); // Retrieving from the database
        }

        public void Save(Student student)
        {
            if (student.Id == 0)  // Setting up the id for new students emulates the ORM behavior
            {
                _lastId++;
                SetId(student, _lastId);
            }

            _existingStudents.RemoveAll(x => x.Id == student.Id); // Saving to the database
            _existingStudents.Add(student);
        }

        private static void SetId(Entity entity, long id)
        {
            entity.GetType().GetProperty(nameof(Entity.Id)).SetValue(entity, id); // The use of reflection to set up the Id emulates the ORM behavior
        }

        private static Student Alice()
        {
            var email = Email.Create("alice@gmail.com");
            var studentName = StudentName.Create("Alice Alison");

            var alice = new Student(email.Value,
                studentName.Value,
                new[] { Address.Create("Rua A", "Ferraz de Vasconcelos", "SP", "08506", new[] { "SP" }).Value }); ;

            SetId(alice, 1);
            alice.Enroll(new Course(1, "Calculus", 5), Grade.A);

            return alice;
        }

        private static Student Bob()
        {
            var email = Email.Create("bob@gmail.com");
            var studentName = StudentName.Create("Bob Bobson");

            var bob = new Student(email.Value,
                studentName.Value,
                new[] { Address.Create("Rua B", "São Paulo", "SP", "01037", new[] { "SP" }).Value });
            SetId(bob, 2);
            bob.Enroll(new Course(2, "History", 4), Grade.B);

            return bob;
        }
    }
}
