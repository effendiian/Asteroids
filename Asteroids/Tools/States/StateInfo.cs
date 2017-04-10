/// StateInfo.cs - Version 3
/// Author: Ian Effendi
/// Date: 4.9.2017

namespace Asteroids.Tools.States
{
	/// <summary>
	/// StateLabel class created in a way to allow string value association with a enum-like behavoir.
	/// This was learned from: http://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp
	/// </summary>
	public class StateInfo
	{

		#region Enum-like Properties. // These static properties utilize the private constructor and allow use like enums.

		/// <summary>
		/// The Menu state label.
		/// </summary>
		public static StateInfo Menu { get { return new StateInfo(StateType.Main); } }

		/// <summary>
		/// The Arena state label.
		/// </summary>
		public static StateInfo Arena { get { return new StateInfo(StateType.Arena); } }

		/// <summary>
		/// The Scores state label.
		/// </summary>
		public static StateInfo Scores { get { return new StateInfo(StateType.Gameover); } }

		/// <summary>
		/// The Options state label.
		/// </summary>
		public static StateInfo Options { get { return new StateInfo(StateType.Options); } }

		/// <summary>
		/// The Pause state label.
		/// </summary>
		public static StateInfo Pause { get { return new StateInfo(StateType.Pause); } }

		/// <summary>
		/// The Quit state label.
		/// </summary>
		public static StateInfo Quit { get { return new StateInfo(StateType.Quit); } }

		/// <summary>
		/// The Quit state label.
		/// </summary>
		public static StateInfo Null { get { return new StateInfo(StateType.Null); } }

		#endregion

		#region Fields. // Contains private data.

		/// <summary>
		/// The StateType's associated string value.
		/// </summary>
		private string label;

		/// <summary>
		/// The StateType for a given state.
		/// </summary>
		private StateType stateType;

		#endregion

		#region Properties. // Publicly accessible data.

		/// <summary>
		/// The string value of the StateType.
		/// </summary>
		public string Label
		{
			get { return this.label; }
			private set { this.label = value; }
		}

		/// <summary>
		/// The StateType type.
		/// </summary>
		public StateType Type
		{
			get { return this.stateType; }
			private set { this.stateType = value; }
		}

		#endregion

		#region Constructor.

		/// <summary>
		/// Private constructor meant to create enum like StateInfo objects.
		/// </summary>
		/// <param name="_stateType">The StateType assigned to an object.</param>
		private StateInfo(StateType _stateType)
		{
			Type = _stateType;
			Label = StateInfo.GetStateTypeAsString(_stateType);
		}

		#endregion

		#region Service methods. // Methods designed to expand class functionality.

		/// <summary>
		/// Returns a StateInfo object matched with a particularly used type.
		/// </summary>
		/// <param name="type">StateType input.</param>
		/// <returns>Returns a StateInfo object.</returns>
		public static StateInfo GetStateInfo(StateType type)
		{
			switch (type)
			{
				case StateType.Arena:
					return StateInfo.Arena;
				case StateType.Gameover:
					return StateInfo.Scores;
				case StateType.Options:
					return StateInfo.Options;
				case StateType.Pause:
					return StateInfo.Pause;
				case StateType.Quit:
					return StateInfo.Quit;
				case StateType.Main:
					return StateInfo.Menu;
				default:
				case StateType.Null:
					return StateInfo.Null;
			}
		}

		/// <summary>
		/// Get the string value associated with a given StateType.
		/// </summary>
		/// <param name="type">StateType to find associated string value for.</param>
		/// <returns>Returns the string value of the input StateType.</returns>
		private static string GetStateTypeAsString(StateType type)
		{
			// http://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp

			switch (type)
			{
				case StateType.Arena:
					return "[Arena]";
				case StateType.Gameover:
					return "[Scores]";
				case StateType.Main:
					return "[Main]";
				case StateType.Options:
					return "[Options]";
				case StateType.Pause:
					return "[Pause]";
				case StateType.Quit:
					return "[Quit]";
				default:
				case StateType.Null:
					return "[Null]";
			}
		}

		#endregion

	}
}
