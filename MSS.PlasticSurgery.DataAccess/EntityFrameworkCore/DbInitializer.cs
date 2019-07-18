using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Entities;

namespace MSS.PlasticSurgery.DataAccess.EntityFrameworkCore
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Operations.Any())
            {
                InitializeOperationsTable(context);
            }

            if (!context.Images.Any())
            {
                InitializeImagesTable(context);
            }
        }

        private static void InitializeOperationsTable(AppDbContext context)
        {
            var operations = new List<Operation>
            {
                new Operation()
                {
                    Id = 1,
                    Title = "Operation 1",
                    Subtitle = "Operation Subtitle 1",
                    Description = "Operation Description 1"
                },
                new Operation()
                {
                    Id = 2,
                    Title = "Operation 2",
                    Subtitle = "Operation Subtitle 2",
                    Description = "Operation Description 2"
                },
                new Operation()
                {
                    Id = 3,
                    Title = "Operation 3",
                    Subtitle = "Operation Subtitle 3",
                    Description = "Operation Description 3"
                }
            };

            context.AddRange(operations);
            context.SaveChanges();
        }

        private static void InitializeImagesTable(AppDbContext context)
        {
            var images = new List<Image>()
            {
                new Image()
                {
                    Id = 1,
                    Path = "~/img/operations/op-1.jpg",
                    OperationId = 1
                },
                new Image()
                {
                    Id = 2,
                    Path = "~/img/operations/op-2.jpg",
                    OperationId = 1
                },
                new Image()
                {
                    Id = 3,
                    Path = "~/img/operations/op-1.jpg",
                    OperationId = 2
                },
                new Image()
                {
                    Id = 4,
                    Path = "~/img/operations/op-2.jpg",
                    OperationId = 2
                },
                new Image()
                {
                    Id = 5,
                    Path = "~/img/operations/op-1.jpg",
                    OperationId = 3
                },
                new Image()
                {
                    Id = 6,
                    Path = "~/img/operations/op-2.jpg",
                    OperationId = 3
                }
            };

            context.AddRange(images);
            context.SaveChanges();
        }
    }
}