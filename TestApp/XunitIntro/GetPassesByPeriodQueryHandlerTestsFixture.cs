/*


using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TCO.TP.DataAccess.CrewChange.Interfaces;
using TCO.TP.Domain.DatabaseEntities.CrewChange.DboScheme;
using TCO.TP.Infrastructure.Implementation.Extensions;
using TCO.TP.Tests;
using TCO.TP.Tests.Extensions;
using TCO.TP.Tests.Extensions.DataAccess;
using TCO.TP.UseCases.V3_2.Passes.Queries.GetPassesByPeriod;
using TCO.TP.UseCases.V3_2.Passes.Queries.GetPassesByPeriodBase.Response;

namespace TCO.TP.UseCases.Tests.V3_2.Passes.Queries.GetPassesByPeriod
{
    public sealed class GetPassesByPeriodQueryHandlerTestsFixture : BaseFixture
    {
        public readonly IMediator Mediator;

        public GetPassesByPeriodQueryHandlerTestsFixture()
        {
            AddService(services => services.AddLocalDbContextFactory());
            AddService(services => services.AddMockCrewChangeDbContextAndFillDatabase(FillCrewChangeDatabase));
            AddService(services => services.AddMockVaccination());
            AddService(services => services.AddInfrastructureServices());
            AddService(services => services.AddMediatR(typeof(Mediator)));
            AddService(services => services.AddTransient<IRequestHandler<GetPassesByPeriodQuery, PassesDto>, GetPassesByPeriodQueryHandler>());

            Mediator = ServiceProvider.GetRequiredService<IMediator>();
        }

        private static void FillCrewChangeDatabase(ICrewChangeDbContext crewChangeDbContext)
        {
            FillDatabaseFor_Handle_Should_Return_Only_Active_Passes(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Only_Passes_That_Have_Entry_And_Exit_Dates(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_TengizVisitors(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Equal_ExitDate(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_Not_TengizVisitors(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Not_Equal_ExitDate(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Passes_With_Correct_Status(crewChangeDbContext);
            FillDatabaseFor_Handle_Should_Return_Paginated_Passes(crewChangeDbContext);

            using var transaction = crewChangeDbContext.Database.BeginTransaction();
            crewChangeDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ParserRequestExceptionForms ON;");
            crewChangeDbContext.SaveChanges();
            crewChangeDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ParserRequestExceptionForms OFF;");
            transaction.Commit();
        }

        private static void FillDatabaseFor_Handle_Should_Return_Only_Active_Passes(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 1,
                    Iin = "123456789001",
                    PassportNo = "PN2905001",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-02-12T23:59:59"),
                    TPPRequestId = 1001,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 2,
                    Iin = "123456789002",
                    PassportNo = "PN2905002",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-02-12T23:59:59"),
                    TPPRequestId = 1002,
                    JustificationForException = "",
                    Status = null
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 3,
                    Iin = "123456789003",
                    PassportNo = "PN2905003",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-02-12T23:59:59"),
                    TPPRequestId = 1003,
                    JustificationForException = "",
                    Status = "Canceled"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 4,
                    Iin = "123456789004",
                    PassportNo = "PN2905004",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-02-12T23:59:59"),
                    TPPRequestId = 1004,
                    JustificationForException = "",
                    Status = "NotActive"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Only_Passes_That_Have_Entry_And_Exit_Dates(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 5,
                    Iin = "123456789005",
                    PassportNo = "PN2905005",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-03-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-03-12T23:59:59"),
                    TPPRequestId = 1005,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 6,
                    Iin = "123456789006",
                    PassportNo = "PN2905006",
                    ExpectedIsolationStartDate = null,
                    TengizDepartureDate = DateTime.Parse("2020-03-12T23:59:59"),
                    TPPRequestId = 1006,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 7,
                    Iin = "123456789007",
                    PassportNo = "PN2905007",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-03-10T00:00:00"),
                    TengizDepartureDate = null,
                    TPPRequestId = 1007,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 8,
                    Iin = "123456789008",
                    PassportNo = "PN2905008",
                    ExpectedIsolationStartDate = null,
                    TengizDepartureDate = null,
                    TPPRequestId = 1008,
                    JustificationForException = "",
                    Status = "Active"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_TengizVisitors(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 9,
                    Iin = "123456789009",
                    PassportNo = "PN2905009",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-04-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-04-25T23:59:59"),
                    TPPRequestId = null,
                    JustificationForException = "TengizVisitor",
                    Status = "Active"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Equal_ExitDate(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 10,
                    Iin = "123456789010",
                    PassportNo = "PN2905010",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-05-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-05-10T23:59:59"),
                    TPPRequestId = 1010,
                    JustificationForException = "",
                    Status = "Active"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_Not_TengizVisitors(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 11,
                    Iin = "123456789011",
                    PassportNo = "PN2905011",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-06-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-06-25T23:59:59"),
                    TPPRequestId = null,
                    JustificationForException = "CrewChange",
                    Status = "Active"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Not_Equal_ExitDate(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 12,
                    Iin = "123456789012",
                    PassportNo = "PN2905012",
                    ExpectedIsolationStartDate = DateTime.Parse("2020-07-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2020-07-25T23:59:59"),
                    TPPRequestId = 1012,
                    JustificationForException = "",
                    Status = "Active"
                });
        }

        private static void FillDatabaseFor_Handle_Should_Return_Passes_With_Correct_Status(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 13,
                    Iin = "123456789013",
                    PassportNo = "PN2905013",
                    ExpectedIsolationStartDate = DateTime.Parse("2000-01-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2000-01-15T23:59:59"),
                    TPPRequestId = 1013,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 14,
                    Iin = "123456789014",
                    PassportNo = "PN2905014",
                    CreatedDate = DateTime.Parse("2000-02-11T00:00:00"),
                    ExpectedIsolationStartDate = DateTime.Parse("2000-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2000-02-15T23:59:59"),
                    TPPRequestId = 1014,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 15,
                    Iin = "123456789015",
                    PassportNo = "PN2905015",
                    CreatedDate = DateTime.Parse("2000-03-01T00:00:00"),
                    ExpectedIsolationStartDate = DateTime.Parse("2000-03-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2000-03-15T23:59:59"),
                    TPPRequestId = 1015,
                    JustificationForException = "",
                    Status = "Active"
                }
            );
        }

        private static void FillDatabaseFor_Handle_Should_Return_Paginated_Passes(
            ICrewChangeDbContext crewChangeDbContext)
        {
            crewChangeDbContext.ParserRequestExceptionForms.AddRange(
                new ParserRequestExceptionFormEntity
                {
                    Id = 16,
                    Iin = "123456789016",
                    PassportNo = "PN2905016",
                    ExpectedIsolationStartDate = DateTime.Parse("2001-01-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2001-01-15T23:59:59"),
                    TPPRequestId = 1016,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 17,
                    Iin = "123456789017",
                    PassportNo = "PN2905017",
                    ExpectedIsolationStartDate = DateTime.Parse("2001-02-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2001-01-16T23:59:59"),
                    TPPRequestId = 1017,
                    JustificationForException = "",
                    Status = "Active"
                },
                new ParserRequestExceptionFormEntity
                {
                    Id = 18,
                    Iin = "123456789018",
                    PassportNo = "PN2905018",
                    ExpectedIsolationStartDate = DateTime.Parse("2001-03-10T00:00:00"),
                    TengizDepartureDate = DateTime.Parse("2001-01-17T23:59:59"),
                    TPPRequestId = 1018,
                    JustificationForException = "",
                    Status = "Active"
                }
            );
        }
    }
}


*/
