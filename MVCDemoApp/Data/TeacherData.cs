using ModelsProject;

namespace MVCDemoApp.Data
{
    internal class TeacherData
    {
        public static List<Teachers> TeachersList = new List<Teachers>() {
            new Teachers {
                Id = 1,
                SubjectId = 1,
                ClassroomId = 1,
                FirstName = "Jahnvi",
                LastName ="Joshi",
                Gender = "F"
            },
            new Teachers {
                Id = 2,
                SubjectId = 2,
                ClassroomId = 2,
                FirstName = "HarshDip",
                LastName ="Patel",
                Gender = "M"
            },
            new Teachers {
                Id = 3,
                SubjectId = 1,
                ClassroomId = 3,
                FirstName = "Kajol",
                LastName ="Sharma",
                Gender = "F"
            },
            new Teachers {
                Id = 4,
                SubjectId = 3,
                ClassroomId = 4,
                FirstName = "Jahnvi",
                LastName ="Joshi",
                Gender = "F"
            },
            new Teachers {
                Id = 5,
                SubjectId = 2,
                ClassroomId = 1,
                FirstName = "HarshDip",
                LastName ="Chudasma",
                Gender = "M"
            },
            new Teachers {
                Id = 6,
                SubjectId = 3,
                ClassroomId = 2,
                FirstName = "Harsh",
                LastName ="Patel",
                Gender = "M"
            },
            new Teachers {
                Id = 7,
                SubjectId = 3,
                ClassroomId = 2,
                FirstName = "HarshDip",
                LastName ="Chudasma",
                Gender = "M"
            }
        };
    }
}
