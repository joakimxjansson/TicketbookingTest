using Moq;

namespace TicketBookingCore.Tests;

public class TicketBookingRequestProcessorTests {
    private readonly TicketBookingRequestProcessor _processor;
    private readonly Mock<ITicketBookingRepository> _ticketBookingRepositoryMock;

   
    public TicketBookingRequestProcessorTests()
    {
        _ticketBookingRepositoryMock = new Mock<ITicketBookingRepository>();
        _processor = new TicketBookingRequestProcessor(_ticketBookingRepositoryMock.Object);
    }
    [Fact]
    public void ShouldReturnTicketBookingResultWithRequestValues() {
        
        var request = new TicketBookingRequest {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
        };
        TicketBookingResponse response = _processor.Book(request);
        
        Assert.NotNull(response);
        Assert.Equal(response.FirstName, request.FirstName);
        Assert.Equal(response.LastName, request.LastName);
        Assert.Equal(response.Email, request.Email);
    }

    [Fact]
    public void ShouldThrowExceptionIfRequestIsNull() {
        //Arrange
      
        //Act
        var exception = Assert.Throws<ArgumentNullException>(() => _processor.Book(null));
        //Assert
        Assert.Equal("request", exception.ParamName);
    }
    [Fact]
    public void ShouldSaveToDatabase() {
        //Arrange
        TicketBooking savedTicketBooking = null;
        _ticketBookingRepositoryMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
            .Callback<TicketBooking>((ticketBooking) =>
            {
                savedTicketBooking = ticketBooking;
            });
        //Act
        var request = new TicketBookingRequest {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
        };
        //
        TicketBookingResponse response = _processor.Book(request);
        
        Assert.NotNull(savedTicketBooking);
        Assert.Equal(request.FirstName, savedTicketBooking.FirstName);
        Assert.Equal(request.LastName, savedTicketBooking.LastName);
        Assert.Equal(request.Email, savedTicketBooking.Email);

    }
    
}

