using SafeVoice.Models;
using Xunit;

namespace SafeVoice.Tests;

public class SupportLocationTests
{
    [Fact]
    public void GetDisplayName_ReturnsCorrectDisplayName()
    {
  
        var gardaDisplayName = SupportLocationType.GardaStation.GetDisplayName();
        var tuslaDisplayName = SupportLocationType.Tusla.GetDisplayName();

        Assert.Equal("Garda Station", gardaDisplayName);
        Assert.Equal("Tusla Office", tuslaDisplayName);
    }

    [Fact]
    public void IsEmergencyService_ReturnsCorrectValue()
    {
        Assert.True(SupportLocationType.GardaStation.IsEmergencyService());
        Assert.True(SupportLocationType.Hospital.IsEmergencyService());
        Assert.False(SupportLocationType.Counselling.IsEmergencyService());
    }
}