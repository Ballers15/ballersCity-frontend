using VavilichevGD.Tools;

namespace SinSity.Domain {
    public class User {
        private static User instance;
        
        public string userId { get; private set; }
        public string authKey { get; private set; }

        public static User GetInstance() {
            return instance ??= new User();
        }

        public void SetUserId(string login) { 
            userId = login;
        }

        public void SetAuthKey(string key) {
            authKey = key;
        }
    }
}