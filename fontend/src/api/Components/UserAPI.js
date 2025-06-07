import BaseAPI from "../base";
class UserAPI extends BaseAPI {
    constructor(){
        super();
        this.controller = "User"
    }
    async Login(param){
        return await this.PostNotAuthen("/authenticate",param);
    }
    async GetUserInfo(){
        return await this.Get("/user-info");
    }
    async InsertUser(param){
        return await this.Post("/insert-user",param);
    }
    async GetUserProfile(){
        return await this.Get("/user-profile");
    }
    async Signup(param){
        return await this.Post("/signup",param);
    }
    async VerifyEmail(param){
        return await this.Post("/verify-email",param);
    }
    async DeleteUser(id)
    {
        return await this.Delete(`delete-user`);
    }
}
export default new UserAPI();