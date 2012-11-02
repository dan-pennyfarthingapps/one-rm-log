using System;
using System.Data;
using SQLite;

namespace onermlog
{
	public class Exercise
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		
		public string Name { get; set; }
		

	}

	// Each Log belongs to an Exercise via Exercise ID
	public class RmLog
	{

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		
		public int ExerciseID { get; set; }

		public DateTime DateLogged { get; set; }

		public double Weight { get; set; }
	}
}

