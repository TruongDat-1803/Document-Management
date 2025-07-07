import BaseAPI from "../base";
import Axios from 'axios';
import store from "@/store/index.js";
class FileAPI extends BaseAPI {
    constructor(){
        super();
        this.controller = "File"
        this.store = store;
    }
    async UploadFile(file){
        return  Axios.post(`http://localhost:44352/api/File/upload-file`, file,
        {
            headers: { 
                'Content-Type': 'multipart/form-data',
                Authorization : `Bearer ${this.store.getters.token}` }
        }).then(response => {
            return response;
        }).catch(error => {
            console.log(error);
        });;
    }
    async GetPersonal(param){
        return await this.Post("/personal",param);
    }
    async GetUnit(param){
        return await this.Post("/unit",param);
    }
    async DeleteElastic(fileId){
        return await this.Delete(`file/${fileId}`);
    }
    async ShareFile(param){
        return await this.Post("/share-file",param);
    }
    async SaveFile(param){
        return await this.Post("/save-file",param);
    }
    async InsertPersonal(param){
        return await this.Post("/insert-personal",param);
    }
    async SearchPersonal(param){
        return await this.Post("/search",JSON.stringify(param));
    }
    async SearchFile(param)
    {
        return await this.Post("/search-file",param);
    }

    async DownloadFile(fileId) {
        return Axios.get(`https://localhost:44352/api/File/download/${fileId}`, {
            responseType: 'blob',
            headers: {
                Authorization: `Bearer ${this.store.getters.token}`
            }
        });
    }
    async GetReceive(param){
        return await this.Post("/receive-file",param);
    }
}
export default new FileAPI();