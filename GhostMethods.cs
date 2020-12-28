namespace PhasmoTestMod
{
    public class GhostMethods
    {
        public static string GhostToString()
        {
            return $"Name: {CustomGhostController.ghosts[CustomGhostController.ghosts.Count - 1].ghostInfo.ghostTraits.ghostName} Type: {CustomGhostController.ghosts[CustomGhostController.ghosts.Count - 1].ghostInfo.ghostTraits.ghostType} Age: {CustomGhostController.ghosts[CustomGhostController.ghosts.Count - 1].ghostInfo.ghostTraits.ghostAge}\n";
        }
    }
}