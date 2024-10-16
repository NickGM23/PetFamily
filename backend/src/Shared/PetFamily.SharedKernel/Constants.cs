namespace PetFamily.SharedKernel
{
    public class Constants
    {
        public const int MAX_LOW_TEXT_LENGTH = 100;

        public const int MAX_HIGH_TEXT_LENGTH = 2000;

        public const int MAX_PHONENUMBER_LENGHT = 20;

        public const int MAX_DATE_LENGHT = 9;

        public static readonly string[] PERMITTED_FILE_EXTENSIONS = [".jpg", ".png"];

        public static readonly string[] PERMITTED_HELP_STATUSES_FROM_VOLUNTEER = ["LookingFoHome", "FoundHome"];
    }
}
