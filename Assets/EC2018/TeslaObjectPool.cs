
namespace EC2018 {
	public class TeslaObjectPool : ExpandableObjectPool {

		public static TeslaObjectPool current;

		void Awake() {
			current = this;
		}

		protected override string GetTag() {
			return Constants.Tags.Tesla;
		}
	}

}
