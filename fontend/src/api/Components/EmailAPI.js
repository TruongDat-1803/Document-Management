import BaseAPI from "../base";
class EmailAPI extends BaseAPI {
    constructor(){
        super();
        this.controller = "Email"
    }
    async VerifyEmail(param){
        return await this.Post("/verify-email",param);
    }
}
export default new EmailAPI();