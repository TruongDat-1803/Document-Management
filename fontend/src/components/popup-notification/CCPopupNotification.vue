<template>
    <div>
        <cc-popup 
            v-model="active"
            :width="400"
            :title="title">
            <div>
                <div v-if="type == 3">
                    <div v-html="content"></div>
                    <div class="flex footer">
                        <cc-button class="m-r-12" type="secondary-border" @click="handleClose">Hủy
                        </cc-button>
                        <cc-button type="delete" @click="handleDelete">
                            <span v-show="!isLoading">{{textDelete}}</span>
                            <cc-loading v-show="isLoading" bdTopColor="#ffffff"></cc-loading>
                        </cc-button>
                    </div>
                </div>
            </div>
        </cc-popup>
    </div>
</template>
<script>
export default {
    data(){
        return{
            title: null,
            active: false,
            type: 1,
            content: "",
            callbackDelete: null,
            dataCallbackDelete: null,
            callbackClose: null,
            dataCallbackClose: null,
            isLoading: false,
            textDelete: "Xóa"
        }
    },
    methods: {
        open(){
            this.active = true;
        },
        async handleDelete(){
            if(this.callbackDelete){
                this.isLoading = true;
                if(this.dataCallbackDelete){
                    await this.callbackDelete(this.dataCallbackDelete);
                }
                else{
                    await this.callbackDelete();
                }
            }
            this.closePopup();
        },
        async handleClose(){
            if(this.callbackClose){
                if(this.dataCallbackClose){
                    await this.callbackDelete(this.dataCallbackClose);
                }
                else{
                    await this.callbackDelete();
                }
            }
            this.closePopup();
        },
        closePopup(){
            this.isLoading = false;
            this.active = false;
        },
        confirmDelete(
            title="Cảnh báo xóa",
            content="Bạn có chắc chắn muốn xóa không?",
            callbackDelete=null,
            dataCallbackDelete=null,
            callbackClose=null,
            dataCallbackClose=null,
            textDelete="Xóa"
        ){
            this.title = title;
            this.content = content;
            this.callbackDelete = callbackDelete;
            this.dataCallbackDelete = dataCallbackDelete;
            this.callbackClose = callbackClose;
            this.dataCallbackClose = dataCallbackClose;
            this.type=3;
            this.textDelete = textDelete;
            this.open();
        }
    }
}
</script>
<style lang="scss">
.footer{
    float: right;
    padding: 12px 0px 4px 0px;
}
</style>