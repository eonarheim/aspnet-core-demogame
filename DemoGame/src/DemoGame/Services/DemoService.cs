namespace DemoGame.Services {

    public interface IDemoService {
        string Test();
    }

    public class DemoService : IDemoService {

        public string Test() {
            return "Foo";
        }

    }

}