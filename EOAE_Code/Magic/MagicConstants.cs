namespace EOAE_Code.Magic
{
    public static class MagicConstants
    {
        // How much exp to give per each healthpoint restored
        public const float RESTORATION_EXP_PER_HEALTHPOINT = 50;

        // Probably not good metric? Maybe transfer exp from summons damage to player or something
        public const float CONJURATION_EXP_PER_SUMMON = 150;

        // Destruction - should be handled by regular weapon damage already?

        //todo: artillery gives throwing - overlaps status effect from missiles, add redirecting skill after both are merges
    }
}
