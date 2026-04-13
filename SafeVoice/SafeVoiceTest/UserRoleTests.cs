using SafeVoice.Models;
using Xunit;

namespace SafeVoice.Tests;

public class UserRoleTests
{
    [Fact]
    public void CanViewAllReports_ReturnsCorrectPermissions()
    {
        Assert.True(UserRole.SuperAdmin.CanViewAllReports());
        Assert.True(UserRole.Garda.CanViewAllReports());
        Assert.True(UserRole.Moderator.CanViewAllReports());
        Assert.False(UserRole.Regular.CanViewAllReports());
    }

    [Fact]
    public void CanManageUsers_OnlySuperAdminCanManage()
    {
        Assert.True(UserRole.SuperAdmin.CanManageUsers());
        Assert.False(UserRole.Garda.CanManageUsers());
        Assert.False(UserRole.Regular.CanManageUsers());
    }
}