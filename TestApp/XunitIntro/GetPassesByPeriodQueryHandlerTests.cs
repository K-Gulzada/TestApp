/*


using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TCO.TP.UseCases.Tests.V3_2.Passes.Queries.GetPassesByPeriod
{
    public class GetPassesByPeriodQueryHandlerTests : IClassFixture<GetPassesByPeriodQueryHandlerTestsFixture>
    {
        private readonly GetPassesByPeriodQueryHandlerTestsFixture _fixture;

        public GetPassesByPeriodQueryHandlerTests(GetPassesByPeriodQueryHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("2020-01-01T05:00:00", "2020-01-05T05:00:00")]
        public async Task Handle_Should_Return_Null(
            DateTime periodFrom, DateTime periodTo)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses.Should().BeNull();
        }

        [Theory]
        [InlineData("2020-02-10T05:00:00", "2020-02-12T05:00:00", new long[] { 1 })]
        public async Task Handle_Should_Return_Only_Active_Passes(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData("2020-03-10T05:00:00", "2020-03-12T05:00:00", new long[] { 5 })]
        public async Task Handle_Should_Return_Only_Passes_That_Have_Entry_And_Exit_Dates(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData("2020-04-09T05:00:00", "2020-04-10T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-10T05:00:00", "2020-04-10T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-09T05:00:00", "2020-04-15T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-10T05:00:00", "2020-04-15T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-24T05:00:00", "2020-04-25T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-25T05:00:00", "2020-04-25T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-24T05:00:00", "2020-04-30T05:00:00", new long[] { 9 })]
        [InlineData("2020-04-25T05:00:00", "2020-04-30T05:00:00", new long[] { 9 })]
        public async Task Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_TengizVisitors(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData("2020-05-09T05:00:00", "2020-05-10T05:00:00", new long[] { 10 })]
        [InlineData("2020-05-10T05:00:00", "2020-05-10T05:00:00", new long[] { 10 })]
        [InlineData("2020-05-09T05:00:00", "2020-05-15T05:00:00", new long[] { 10 })]
        [InlineData("2020-05-10T05:00:00", "2020-05-15T05:00:00", new long[] { 10 })]
        public async Task Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Equal_ExitDate(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData("2020-06-08T05:00:00", "2020-06-08T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-10T05:00:00", "2020-06-08T05:00:00", new long[] { 11 })]//10 > 08 ?
        [InlineData("2020-06-12T05:00:00", "2020-06-08T05:00:00", new long[] { 11 })]//12 > 08 ?
        [InlineData("2020-06-10T05:00:00", "2020-06-10T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-12T05:00:00", "2020-06-10T05:00:00", new long[] { 11 })]//10 > 10 ?
        [InlineData("2020-06-10T05:00:00", "2020-06-12T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-23T05:00:00", "2020-06-23T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-23T05:00:00", "2020-06-25T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-23T05:00:00", "2020-06-30T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-25T05:00:00", "2020-06-23T05:00:00", new long[] { 11 })]//25 > 23 ?
        [InlineData("2020-06-25T05:00:00", "2020-06-25T05:00:00", new long[] { 11 })]
        [InlineData("2020-06-25T05:00:00", "2020-06-30T05:00:00", new long[] { 11 })]//25 > 30 ?
        [InlineData("2020-06-27T05:00:00", "2020-06-23T05:00:00", new long[] { 11 })]//27 > 23 ?
        [InlineData("2020-06-27T05:00:00", "2020-06-25T05:00:00", new long[] { 11 })]//27 > 25 ?
        [InlineData("2020-06-27T05:00:00", "2020-06-30T05:00:00", new long[] { 11 })]
        public async Task Handle_Should_Return_Passes_Created_From_CrewChangePortal_For_Not_TengizVisitors(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData("2020-07-08T05:00:00", "2020-07-08T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-10T05:00:00", "2020-07-08T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-12T05:00:00", "2020-07-08T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-10T05:00:00", "2020-07-10T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-12T05:00:00", "2020-07-10T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-10T05:00:00", "2020-07-12T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-23T05:00:00", "2020-07-23T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-23T05:00:00", "2020-07-25T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-23T05:00:00", "2020-07-30T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-25T05:00:00", "2020-07-23T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-25T05:00:00", "2020-07-25T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-25T05:00:00", "2020-07-30T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-27T05:00:00", "2020-07-23T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-27T05:00:00", "2020-07-25T05:00:00", new long[] { 12 })]
        [InlineData("2020-07-27T05:00:00", "2020-07-30T05:00:00", new long[] { 12 })]
        public async Task Handle_Should_Return_Passes_Created_From_TengizPassPortal_And_EntryDate_Not_Equal_ExitDate(
            DateTime periodFrom, DateTime periodTo, long[] expectedPassIds)
        {
            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, 1, int.MaxValue);
            var actualPasses = await _fixture.Mediator.Send(query);

            actualPasses
                .Should().NotBeNull()
                .And
                .Subject.As<PassesDto>()
                .PassInformation.Select(pi => pi.Id).Should().BeEquivalentTo(expectedPassIds);
        }

        [Theory]
        [InlineData(null, "2000-01-16T05:00:00", PassStatus.New)]
        [InlineData("2000-02-10T05:00:00", "2000-02-16T05:00:00", PassStatus.New)]
        [InlineData("2000-03-10T05:00:00", "2000-03-16T05:00:00", PassStatus.Updated)]
        public async Task Handle_Should_Return_Passes_With_Correct_Status(
            string periodFrom, string periodTo, PassStatus expectedStatus)
        {
            DateTime? periodFromNullableDateTime = !string.IsNullOrEmpty(periodFrom) ? DateTime.Parse(periodFrom) : null;

            var query = new GetPassesByPeriodQuery(periodFromNullableDateTime, DateTime.Parse(periodTo), 1, int.MaxValue);
            var passes = await _fixture.Mediator.Send(query);

            var actualStatuses = passes.PassInformation.Select(pi => pi.Status);
            actualStatuses.Should().NotBeNullOrEmpty().And.OnlyContain(ps => ps == expectedStatus);
        }

        [Theory]
        [InlineData(1, 1, 3)]
        [InlineData(1, 2, 2)]
        [InlineData(1, 3, 1)]
        public async Task Handle_Should_Return_Paginated_Passes(
            int pageIndex, int pageSize, int actualPageCount)
        {
            var periodFrom = DateTime.Parse("2001-01-10T05:00:00");
            var periodTo = DateTime.Parse("2001-01-17T05:00:00");

            var query = new GetPassesByPeriodQuery(periodFrom, periodTo, pageIndex, pageSize);
            var passes = await _fixture.Mediator.Send(query);

            var expectedPageCount = passes.PagingInformation.PageCount;
            expectedPageCount.Should().Be(actualPageCount);
        }
    }
}


*/