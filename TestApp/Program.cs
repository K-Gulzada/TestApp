
using TestApp.LinqSpecsIntro.Repositories;


ProjectRepository projectRepository = new ProjectRepository();

projectRepository.Test();

// projectRepository.GetActiveProjects();
// projectRepository.GetByDescription("1");
// projectRepository.GetByNumber("5555");
// projectRepository.GetById(6);



/*
 
var mockDateTimeService = new Mock<IDateTimeService>();
mockDateTimeService.Setup(dts => dts.UtcNow).Returns(new DateTimeOffset(2022, 07, 19, 10, 15, 20, 500, new TimeSpan(0, 0, 0)));
dateTimeService = mockDateTimeService.Object;



    AfeNumber = "456456541",
    AfeDescription = "Lorem Ipsum.",
    ...

    CreatedDate = dateTimeService.Now

*/