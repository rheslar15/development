using System;

namespace LiRoInspect.iOS
{
	public static class Constants
	{
		/// <summary>
		/// Adding N/A for all the options except Special and DOB Progress Inspection
		/// </summary>
		public const int DOB_SPECIALPROGRESS_SEQUENCEID=8;

		/// <summary>
		/// Guided Pictures Results
		/// </summary>
		public const string GUIDED_PICTURE_RESULT="guided";

		/// <summary>
		/// maximum Images for Inspection
		/// </summary>
		public const int INSPECTION_MAXIMAGE=9;

		/// <summary>
		/// Final Inspection ID
		/// </summary>
		public const string FINAL_INSPECTIONID="7";

		/// <summary>
		/// 90% Inspection ID
		/// </summary>
		public const string NINTY_PERCENT_INSPECTIONID="6";

		/// <summary>
		/// Guided Option ID
		/// </summary>
		public const int GUIDED_OPTIONID=26;

		/// <summary>
		/// FINAL PUNCH Option ID
		/// </summary>
		public const int FINALPUNCH_OPTIONID=25;

		/// <summary>
		/// FINAL PUNCH Sequence ID
		/// </summary>
		public const int FINALPUNCH_SEQUENCEID=9;

		/// <summary>
		/// DBVersion string
		/// </summary>
		public const string DB_Version="DBVersion";

		/// <summary>
		/// Regular expression for DBVersion Validation
		/// </summary>
		public const string DBVersion_RegEx="[0-9]*[.]|[0-9]";

		/// <summary>
		/// Photo Log Report Type
		/// </summary>
		public const string REPORTTYPE_PHOTOLOG="photoLog";


		/// <summary>
		/// Admin User ID and password 
		/// </summary>
		public const string AdminUserID="ADMIN";
		public const string AdminPassword="ADMIN";
	}
}