using Pinguin.Models;

namespace Pinguin.Test.Models;

[TestFixture]
public class UpdateTest
{
    [Test]
    public async Task CheckForUpdates_NewerVersionExists_True()
    {
        var update = await UpdateChecker.CheckForUpdates("0.0.1");
        if (!update) Assert.Fail();
    }

    [Test]
    public async Task CheckForUpdates_NewerVersionNotExists_True()
    {
        // this will fail if I don't update it lol.
        // feel like there's gotta be a better way to do this but ehh if it fails just bump this lol
        var update = await UpdateChecker.CheckForUpdates("0.0.2");
        if (update) Assert.Fail();
    }
}