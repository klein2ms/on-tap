using System.Collections.Generic;
using AutoMapper;
using OnTap.Blazor.Server.Models;
using Xunit;


namespace OnTap.Blazor.Server.Tests
{
    public class AutoMapperTests
    {        
        [Fact]
        public void LeagueProfile_AssertConfigurationIsValid_ReturnsTrue()
            => AssertConfigurationIsValid(new List<Profile> 
            { 
                new LeagueProfile(), 
                new LeagueSettingsProfile(), 
                new TeamProfile() 
            });

        [Fact]
        public void TeamProfile_AssertConfigurationIsValid_ReturnsTrue()
            => AssertConfigurationIsValid(new [] { new TeamProfile() });
        
        [Fact]
        public void DraftProfile_AssertConfigurationIsValid_ReturnsTrue()
            => AssertConfigurationIsValid(new List<Profile>
            {
                new DraftProfile(),
                new LeagueProfile(),
                new LeagueSettingsProfile(), 
                new TeamProfile(), 
                new DraftPickProfile(),
                new DraftPositionProfile(),
                new PlayerProfile()
            });

        private static void AssertConfigurationIsValid(IEnumerable<Profile> profiles)
        {
            var sut = new MapperConfiguration(c =>
            {
                c.AddProfiles(profiles);
            });

            sut.AssertConfigurationIsValid();  
        }
    }
}
