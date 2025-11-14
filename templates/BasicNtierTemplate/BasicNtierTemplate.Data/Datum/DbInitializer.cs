using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Data.Datum
{
    public static class DbInitializer
    {
        public static void Initialize(BasicNtierTemplateDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            // Look for any students.
            if (dbContext.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = GetStudents();
            foreach (Student s in students)
            {
                dbContext.Students.Add(s);
            }
            dbContext.SaveChanges();

            var courses = GetCourses();
            foreach (Course c in courses)
            {
                dbContext.Courses.Add(c);
            }
            dbContext.SaveChanges();

            var enrollments = GetEnrollments();
            foreach (Enrollment e in enrollments)
            {
                dbContext.Enrollments.Add(e);
            }
            dbContext.SaveChanges();
        }

        private static IEnumerable<Enrollment> GetEnrollments()
        {
            return
                [
                new Enrollment{ Id = 1, CourseId = 1050, Grade = Grade.A },
                new Enrollment{ Id = 1, CourseId = 4022, Grade = Grade.C },
                new Enrollment{ Id = 1, CourseId = 4041, Grade = Grade.B },
                new Enrollment{ Id = 2, CourseId = 1045, Grade = Grade.B },
                new Enrollment{ Id = 2, CourseId = 3141, Grade = Grade.F },
                new Enrollment{ Id = 2, CourseId = 2021, Grade = Grade.F },
                new Enrollment{ Id = 3, CourseId = 1050 },
                new Enrollment{ Id = 4, CourseId = 1050 },
                new Enrollment{ Id = 4, CourseId = 4022, Grade = Grade.F },
                new Enrollment{ Id = 5, CourseId = 4041, Grade = Grade.C },
                new Enrollment{ Id = 6, CourseId = 1045 },
                new Enrollment{ Id = 7, CourseId = 3141, Grade = Grade.A }
                ];
        }

        private static IEnumerable<Course> GetCourses()
        {
            return
                [
                new Course { Id = 1050, Title = "Chemistry", Credits = 3 },
                new Course { Id = 4022, Title = "Microeconomics", Credits = 3 },
                new Course { Id = 4041, Title = "Macroeconomics", Credits = 3 },
                new Course { Id = 1045, Title = "Calculus", Credits = 4 },
                new Course { Id = 3141, Title = "Trigonometry", Credits = 4 },
                new Course { Id = 2021, Title = "Composition", Credits = 3 },
                new Course { Id = 2042, Title = "Literature", Credits = 4 }
               ];
        }

        private static IEnumerable<Student> GetStudents()
        {
            return
                [
                new Student{ FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("1999-09-01 10:23:45") },
                new Student{ FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2021-09-01 11:15:32") },
                new Student{ FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2003-09-01 12:47:18") },
                new Student{ FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("1999-09-01 10:55:23") },
                new Student{ FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2002-09-01 11:32:07") },
                new Student{ FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2001-09-01 10:18:54") },
                new Student{ FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2003-09-01 12:25:41") },
                new Student{ FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2010-09-01 11:08:19") },
                new Student{ FirstMidName = "Brandon", LastName = "Mitchell", EnrollmentDate = DateTime.Parse("1995-03-15 10:42:33") },
                new Student{ FirstMidName = "Emily", LastName = "Johnson", EnrollmentDate = DateTime.Parse("2018-01-22 11:55:12") },
                new Student{ FirstMidName = "Michael", LastName = "Davis", EnrollmentDate = DateTime.Parse("2007-06-10 12:14:28") },
                new Student{ FirstMidName = "Sarah", LastName = "Wilson", EnrollmentDate = DateTime.Parse("2012-11-03 10:37:56") },
                new Student{ FirstMidName = "Joshua", LastName = "Martinez", EnrollmentDate = DateTime.Parse("1998-08-17 11:21:44") },
                new Student{ FirstMidName = "Ashley", LastName = "Anderson", EnrollmentDate = DateTime.Parse("2015-04-29 12:03:15") },
                new Student{ FirstMidName = "Christopher", LastName = "Taylor", EnrollmentDate = DateTime.Parse("2001-12-08 10:49:37") },
                new Student{ FirstMidName = "Jessica", LastName = "Thomas", EnrollmentDate = DateTime.Parse("2019-07-14 11:26:09") },
                new Student{ FirstMidName = "Matthew", LastName = "Jackson", EnrollmentDate = DateTime.Parse("2005-02-21 12:38:52") },
                new Student{ FirstMidName = "Amanda", LastName = "White", EnrollmentDate = DateTime.Parse("2013-10-06 10:11:25") },
                new Student{ FirstMidName = "Daniel", LastName = "Harris", EnrollmentDate = DateTime.Parse("1997-05-19 11:44:18") },
                new Student{ FirstMidName = "Stephanie", LastName = "Martin", EnrollmentDate = DateTime.Parse("2020-09-27 12:22:46") },
                new Student{ FirstMidName = "David", LastName = "Thompson", EnrollmentDate = DateTime.Parse("2008-03-12 10:58:31") },
                new Student{ FirstMidName = "Jennifer", LastName = "Garcia", EnrollmentDate = DateTime.Parse("2016-11-25 11:17:04") },
                new Student{ FirstMidName = "Andrew", LastName = "Martinez", EnrollmentDate = DateTime.Parse("2002-08-03 12:41:59") },
                new Student{ FirstMidName = "Nicole", LastName = "Robinson", EnrollmentDate = DateTime.Parse("2011-01-16 10:28:14") },
                new Student{ FirstMidName = "Ryan", LastName = "Clark", EnrollmentDate = DateTime.Parse("1999-06-30 11:53:27") },
                new Student{ FirstMidName = "Samantha", LastName = "Rodriguez", EnrollmentDate = DateTime.Parse("2017-04-08 12:09:43") },
                new Student{ FirstMidName = "Justin", LastName = "Lewis", EnrollmentDate = DateTime.Parse("2004-12-19 10:35:58") },
                new Student{ FirstMidName = "Elizabeth", LastName = "Lee", EnrollmentDate = DateTime.Parse("2014-07-02 11:48:21") },
                new Student{ FirstMidName = "Kevin", LastName = "Walker", EnrollmentDate = DateTime.Parse("2000-02-14 12:16:37") },
                new Student{ FirstMidName = "Heather", LastName = "Hall", EnrollmentDate = DateTime.Parse("2009-10-28 10:44:52") },
                new Student{ FirstMidName = "Brian", LastName = "Allen", EnrollmentDate = DateTime.Parse("2022-05-11 11:29:06") },
                new Student{ FirstMidName = "Michelle", LastName = "Young", EnrollmentDate = DateTime.Parse("1996-09-23 12:52:19") },
                new Student{ FirstMidName = "Jason", LastName = "Hernandez", EnrollmentDate = DateTime.Parse("2006-03-07 10:07:34") },
                new Student{ FirstMidName = "Melissa", LastName = "King", EnrollmentDate = DateTime.Parse("2015-11-20 11:39:48") },
                new Student{ FirstMidName = "James", LastName = "Wright", EnrollmentDate = DateTime.Parse("2001-08-04 12:24:11") },
                new Student{ FirstMidName = "Rebecca", LastName = "Lopez", EnrollmentDate = DateTime.Parse("2019-01-17 10:51:26") },
                new Student{ FirstMidName = "Tyler", LastName = "Hill", EnrollmentDate = DateTime.Parse("2010-06-29 11:13:42") },
                new Student{ FirstMidName = "Amy", LastName = "Scott", EnrollmentDate = DateTime.Parse("1998-12-12 12:46:57") },
                new Student{ FirstMidName = "Jacob", LastName = "Green", EnrollmentDate = DateTime.Parse("2023-04-25 10:22:13") },
                new Student{ FirstMidName = "Rachel", LastName = "Adams", EnrollmentDate = DateTime.Parse("2007-09-08 11:57:28") },
                new Student{ FirstMidName = "Nicholas", LastName = "Baker", EnrollmentDate = DateTime.Parse("2016-02-20 12:31:44") },
                new Student{ FirstMidName = "Kimberly", LastName = "Gonzalez", EnrollmentDate = DateTime.Parse("2003-07-14 10:14:59") },
                new Student{ FirstMidName = "Jonathan", LastName = "Nelson", EnrollmentDate = DateTime.Parse("2012-12-26 11:42:15") },
                new Student{ FirstMidName = "Lauren", LastName = "Carter", EnrollmentDate = DateTime.Parse("2000-05-09 12:19:31") },
                new Student{ FirstMidName = "Brandon", LastName = "Mitchell", EnrollmentDate = DateTime.Parse("2021-10-22 10:56:47") },
                new Student{ FirstMidName = "Brittany", LastName = "Perez", EnrollmentDate = DateTime.Parse("1997-03-05 11:34:02") },
                new Student{ FirstMidName = "Austin", LastName = "Roberts", EnrollmentDate = DateTime.Parse("2008-08-18 12:08:18") },
                new Student{ FirstMidName = "Victoria", LastName = "Turner", EnrollmentDate = DateTime.Parse("2018-01-30 10:45:34") },
                new Student{ FirstMidName = "Zachary", LastName = "Phillips", EnrollmentDate = DateTime.Parse("2005-06-13 11:23:49") },
                new Student{ FirstMidName = "Christina", LastName = "Campbell", EnrollmentDate = DateTime.Parse("2013-11-26 12:58:05") },
                new Student{ FirstMidName = "Ethan", LastName = "Parker", EnrollmentDate = DateTime.Parse("2002-04-09 10:19:21") },
                new Student{ FirstMidName = "Danielle", LastName = "Evans", EnrollmentDate = DateTime.Parse("2020-09-21 11:52:36") },
                new Student{ FirstMidName = "Nathan", LastName = "Edwards", EnrollmentDate = DateTime.Parse("1999-02-03 12:27:52") },
                new Student{ FirstMidName = "Amber", LastName = "Collins", EnrollmentDate = DateTime.Parse("2009-07-17 10:04:08") },
                new Student{ FirstMidName = "Kyle", LastName = "Stewart", EnrollmentDate = DateTime.Parse("2017-12-30 11:41:23") },
                new Student{ FirstMidName = "Alexis", LastName = "Sanchez", EnrollmentDate = DateTime.Parse("2004-05-12 12:15:39") },
                new Student{ FirstMidName = "Cody", LastName = "Morris", EnrollmentDate = DateTime.Parse("2014-10-25 10:52:54") },
                new Student{ FirstMidName = "Courtney", LastName = "Rogers", EnrollmentDate = DateTime.Parse("2001-03-08 11:30:10") },
                new Student{ FirstMidName = "Aaron", LastName = "Reed", EnrollmentDate = DateTime.Parse("2011-08-21 12:06:26") },
                new Student{ FirstMidName = "Kayla", LastName = "Cook", EnrollmentDate = DateTime.Parse("1998-01-04 10:43:41") },
                new Student{ FirstMidName = "Jordan", LastName = "Morgan", EnrollmentDate = DateTime.Parse("2019-06-17 11:21:57") },
                new Student{ FirstMidName = "Kristen", LastName = "Bell", EnrollmentDate = DateTime.Parse("2006-11-29 12:54:12") },
                new Student{ FirstMidName = "Dylan", LastName = "Murphy", EnrollmentDate = DateTime.Parse("2015-04-13 10:28:28") },
                new Student{ FirstMidName = "Taylor", LastName = "Bailey", EnrollmentDate = DateTime.Parse("2003-09-26 11:05:44") },
                new Student{ FirstMidName = "Morgan", LastName = "Rivera", EnrollmentDate = DateTime.Parse("2012-02-08 12:39:59") },
                new Student{ FirstMidName = "Travis", LastName = "Cooper", EnrollmentDate = DateTime.Parse("2000-07-22 10:17:15") },
                new Student{ FirstMidName = "Hannah", LastName = "Richardson", EnrollmentDate = DateTime.Parse("2021-12-04 11:54:31") },
                new Student{ FirstMidName = "Adam", LastName = "Cox", EnrollmentDate = DateTime.Parse("1997-05-18 12:28:46") },
                new Student{ FirstMidName = "Sydney", LastName = "Howard", EnrollmentDate = DateTime.Parse("2008-10-31 10:06:02") },
                new Student{ FirstMidName = "Lucas", LastName = "Ward", EnrollmentDate = DateTime.Parse("2018-03-15 11:43:18") },
                new Student{ FirstMidName = "Allison", LastName = "Torres", EnrollmentDate = DateTime.Parse("2005-08-28 12:17:33") },
                new Student{ FirstMidName = "Mason", LastName = "Peterson", EnrollmentDate = DateTime.Parse("2013-01-10 10:54:49") },
                new Student{ FirstMidName = "Olivia", LastName = "Gray", EnrollmentDate = DateTime.Parse("2002-06-24 11:32:05") },
                new Student{ FirstMidName = "Logan", LastName = "Ramirez", EnrollmentDate = DateTime.Parse("2020-11-06 12:09:20") },
                new Student{ FirstMidName = "Emma", LastName = "James", EnrollmentDate = DateTime.Parse("1999-04-20 10:46:36") },
                new Student{ FirstMidName = "Connor", LastName = "Watson", EnrollmentDate = DateTime.Parse("2010-09-02 11:23:52") },
                new Student{ FirstMidName = "Sophia", LastName = "Brooks", EnrollmentDate = DateTime.Parse("2007-02-15 12:58:07") },
                new Student{ FirstMidName = "Liam", LastName = "Kelly", EnrollmentDate = DateTime.Parse("2016-07-29 10:35:23") },
                new Student{ FirstMidName = "Isabella", LastName = "Sanders", EnrollmentDate = DateTime.Parse("2004-12-11 11:12:39") },
                new Student{ FirstMidName = "Noah", LastName = "Price", EnrollmentDate = DateTime.Parse("2014-05-24 12:47:54") },
                new Student{ FirstMidName = "Ava", LastName = "Bennett", EnrollmentDate = DateTime.Parse("2001-10-07 10:25:10") },
                new Student{ FirstMidName = "William", LastName = "Wood", EnrollmentDate = DateTime.Parse("2011-03-21 11:02:26") },
                new Student{ FirstMidName = "Mia", LastName = "Barnes", EnrollmentDate = DateTime.Parse("1998-08-04 12:36:41") },
                new Student{ FirstMidName = "Benjamin", LastName = "Ross", EnrollmentDate = DateTime.Parse("2019-01-17 10:13:57") },
                new Student{ FirstMidName = "Charlotte", LastName = "Henderson", EnrollmentDate = DateTime.Parse("2006-06-30 11:51:13") },
                new Student{ FirstMidName = "Elijah", LastName = "Coleman", EnrollmentDate = DateTime.Parse("2015-12-13 12:25:28") },
                new Student{ FirstMidName = "Amelia", LastName = "Jenkins", EnrollmentDate = DateTime.Parse("2003-05-27 10:02:44") },
                new Student{ FirstMidName = "Oliver", LastName = "Perry", EnrollmentDate = DateTime.Parse("2012-10-09 11:40:00") },
                new Student{ FirstMidName = "Harper", LastName = "Powell", EnrollmentDate = DateTime.Parse("2000-03-23 12:14:15") },
                new Student{ FirstMidName = "Henry", LastName = "Long", EnrollmentDate = DateTime.Parse("2021-08-05 10:51:31") },
                new Student{ FirstMidName = "Evelyn", LastName = "Patterson", EnrollmentDate = DateTime.Parse("1997-01-19 11:28:47") },
                new Student{ FirstMidName = "Alexander", LastName = "Hughes", EnrollmentDate = DateTime.Parse("2009-06-03 12:03:02") },
                new Student{ FirstMidName = "Abigail", LastName = "Flores", EnrollmentDate = DateTime.Parse("2017-11-16 10:40:18") },
                new Student{ FirstMidName = "Sebastian", LastName = "Washington", EnrollmentDate = DateTime.Parse("2005-04-30 11:17:34") },
                new Student{ FirstMidName = "Emily", LastName = "Butler", EnrollmentDate = DateTime.Parse("2013-09-12 12:51:49") },
                new Student{ FirstMidName = "Jackson", LastName = "Simmons", EnrollmentDate = DateTime.Parse("2002-02-26 10:29:05") },
                new Student{ FirstMidName = "Ella", LastName = "Foster", EnrollmentDate = DateTime.Parse("2020-07-10 11:06:21") },
                new Student{ FirstMidName = "Aiden", LastName = "Gonzales", EnrollmentDate = DateTime.Parse("1999-12-23 12:40:36") }
                ];
        }
    }
}
