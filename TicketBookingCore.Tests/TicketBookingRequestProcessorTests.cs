using Moq;

namespace TicketBookingCore.Tests;

public class TicketBookingRequestProcessorTests {
    private readonly TicketBookingRequestProcessor _processor;
    private readonly Mock<ITicketBookingRepository> _ticketBookingRepositoryMock;
    private readonly TicketBookingRequest _request;
    

   
    public TicketBookingRequestProcessorTests()
    {
        _ticketBookingRepositoryMock = new Mock<ITicketBookingRepository>();
        _processor = new TicketBookingRequestProcessor(_ticketBookingRepositoryMock.Object);
        _request = new TicketBookingRequest();
       
    }
    [Fact]
    public void ShouldReturnTicketBookingResultWithRequestValues() {
        
       
        TicketBookingResponse response = _processor.Book(_request);
        
        Assert.NotNull(response);
        Assert.Equal(response.FirstName, _request.FirstName);
        Assert.Equal(response.LastName, _request.LastName);
        Assert.Equal(response.Email, _request.Email);
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
      
        //
        TicketBookingResponse response = _processor.Book(_request);
        
        Assert.NotNull(savedTicketBooking);
        Assert.Equal(_request.FirstName, savedTicketBooking.FirstName);
        Assert.Equal(_request.LastName, savedTicketBooking.LastName);
        Assert.Equal(_request.Email, savedTicketBooking.Email);

    }
    
}

