using Ediiie.Model;
using System;

namespace Ediiie.Model
{
    public class GetUserProfileModel : UserModel<GetUserProfileRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(GetUserProfileRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            SetForm(data);
            CallAPI(url, wwwForm, api_name);
        }

        protected override void SetForm(GetUserProfileRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("user_id", User.id);
        }
    }

    public class GetUserProfileRequest
    {

    }

    [Serializable]
    public class GetUserProfileResponse : APIResult<int>
    {
        public int credits;
    }
}
