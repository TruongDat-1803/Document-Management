<template>
    <div class="add-role">
        <div class="flex header">
            <div class="flex">
                <div class="btn-icon-2 m-r-16" @click="back">
                    <div class="icon-back"></div>
                </div>
                <div class="title" v-if="roleMaster == null">Thêm vai trò</div>
                <div class="title" v-else>{{role.RoleName}}</div>
            </div>
            <div class="flex" v-if="roleMaster == null">
                <cc-button class="m-r-12" type="secondary" @click="back">Hủy</cc-button>
                <cc-button @click="save" :loading="loading">Lưu</cc-button>
            </div>
        </div>
        <div class="content">
            <cc-group title="Thông tin vai trò" class="m-b-24">
                <cc-row>
                    <cc-col w="15">
                        Tên vai trò <span class="required">*</span>
                    </cc-col>
                    <cc-col w="30" class="m-r-60">
                        <div class="input-wrapper">
                            <cc-input v-if="roleMaster == null" v-model="role.RoleName" :error="errorRoleName" :required="true"></cc-input>
                            <div v-else>{{role.RoleName}}</div>
                            <div v-if="errorRoleName" class="error-message">{{errorRoleName}}</div>
                        </div>
                    </cc-col>
                    <cc-col w="15">
                        Ghi chú
                    </cc-col>
                    <cc-col w="30">
                        <cc-input v-if="roleMaster == null" v-model="role.Note"></cc-input>
                        <div v-else>{{role.Note}}</div>
                    </cc-col>
                </cc-row>
            </cc-group>
            <cc-group title="Danh sách quyền chức năng">
                <div v-for="(item, index) in role.ListRole" :key="index" class="flex m-b-24">
                    <div class="role-name">
                        {{item.SubName}}
                    </div>
                    <div class="flex">
                        <div v-for="(role, indexRole) in item.ListRoleDetail" :key="indexRole" class="item">
                            <cc-check-box :modelValue="role.Value" @update:modelValue="(val) => role.Value = val" :text="role.ActionName" :disabled="roleMaster != null"></cc-check-box>
                        </div>
                    </div>
                </div>
            </cc-group>
        </div>
    </div>
</template>
<script>
import RoleAPI from '@/api/Components/RoleAPI.js';
import RoleDetailAPI from '@/api/Components/RoleDetailAPI.js';
export default {
    props: {
        roleMaster: {
            type: Object,
            default: null
        }
    },
    data(){
        return{
            role: {
                RoleID: null,
                ActionName: null,
                Note: null,
                RoleCode: null,
                ListRole: [
                    {
                        SubCode: "OrganizationUnit",
                        SubName: "Đơn vị",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            },
                            {
                                Value: false,
                                ActionName: "Tải xuống",
                                Action: "Download"
                            },
                        ]
                    },
                    {
                        SubCode: "Personal",
                        SubName: "Cá nhân",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            },
                            {
                                Value: false,
                                ActionName: "Tải xuống",
                                Action: "Download"
                            },
                        ]
                    },
                    {
                        SubCode: "Shared",
                        SubName: "Chia sẻ",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            },
                            {
                                Value: false,
                                ActionName: "Tải xuống",
                                Action: "Download"
                            },
                        ]
                    },
                    {
                        SubCode: "Employee",
                        SubName: "Thiết lập/Nhân viên",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Sửa",
                                Action: "Edit"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            }
                        ]
                    },
                    {
                        SubCode: "Unit",
                        SubName: "Thiết lập/Đơn vị",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Sửa",
                                Action: "Edit"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            }
                        ]
                    },
                    {
                        SubCode: "User",
                        SubName: "Thiết lập/Người dùng",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Sửa",
                                Action: "Edit"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            }
                        ]
                    },
                    {
                        SubCode: "Role",
                        SubName: "Thiết lập/Vai trò",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Thêm",
                                Action: "Add"
                            },
                            {
                                Value: false,
                                ActionName: "Sửa",
                                Action: "Edit"
                            },
                            {
                                Value: false,
                                ActionName: "Xóa",
                                Action: "Delete"
                            }
                        ]
                    },
                    {
                        SubCode: "Email",
                        SubName: "Thiết lập/Email hệ thống",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            },
                            {
                                Value: false,
                                ActionName: "Sửa",
                                Action: "Edit"
                            },
                        ]
                    },
                    {
                        SubCode: "Log",
                        SubName: "Thiết lập/Nhật ký hoạt động",
                        ListRoleDetail: [
                            {
                                Value: false,
                                ActionName: "Xem",
                                Action: "View"
                            }
                        ]
                    }
                ]

            },
            loading: false,
            errorRoleName: ""
        }
    },
    watch: {
        roleMaster: {
            handler(val){
                if(val != null){
                    var me = this;
                    this.role.RoleName = val.RoleName;
                    this.role.Note = val.Note;
                    RoleDetailAPI.GetListRoleDetail(val.RoleID).then(res => {
                        if(res.data && res.data.Success){
                            var listData = res.data.Data;
                            //Buil danh sách giá trị
                            me.role.ListRole.forEach(roles => {
                                roles.ListRoleDetail.forEach(role => {
                                    var item = listData.find(x => x.SubCode == roles.SubCode && x.Action == role.Action);
                                    if(item != null){
                                        role.Value = item.Value;
                                    }
                                    else{
                                        role.Value = false;
                                    }
                                })
                            })
                        }
                    })
                }
            },
            immediate: true
        }
    },
    methods: {
        back(){
            this.$emit("close", true);
        },
        async save(){
            try{
                var me = this;
                // Validate
                if(!this.role.RoleName || this.role.RoleName.trim() === ""){
                    this.errorRoleName = "Vui lòng nhập tên vai trò";
                    return;
                }
                if(this.role.RoleName.length > 100){
                    this.errorRoleName = "Tên vai trò không được vượt quá 100 ký tự";
                    return;
                }
                this.errorRoleName = "";
                
                //Buil danh sách Quyền để thêm
                var listData = [];
                me.role.ListRole.forEach(roles => {
                    roles.ListRoleDetail.forEach(role => {
                        var item = {
                            SubCode: roles.SubCode,
                            SubName: roles.SubName,
                            Action: role.Action,
                            ActionName: role.ActionName,
                            Value: role.Value,
                            State: 1,
                            
                        };
                        listData.push(item);
                    })
                })
                var param = {
                    RoleName: this.role.RoleName,
                    Note: this.role.Note,
                    TenantID: this.$store.getters.tenantID,
                    ListRole: listData
                }
                console.log(param);
                debugger;
                this.loading = true;
                var res = await RoleDetailAPI.SaveList(param);
                if(res.data && res.data.Success){
                    this.$notify({
                        title: 'Thành công',
                        message: 'Thêm vai trò thành công',
                        type: 'success'
                    });
                    me.$emit("save", true);
                }
                else{
                    this.$notify({
                        title: 'Thất bại',
                        message: 'Thêm vai trò thất bại',
                        type: 'error'
                    });
                    me.loading = false;
                }
            }
            catch(e){
                this.$notify({
                    title: 'Lỗi',
                    message: 'Có lỗi xảy ra khi thêm vai trò',
                    type: 'error'
                });
                me.loading = false;
            }
        }
    }
}
</script>
<style lang="scss" scoped>
.header{
    justify-content: space-between;
}
.add-role{
    height: calc(100% - 60px);
    .header{
        .title{
            font-size: 22px;
            font-weight: 500;
            align-items: center;
            display: flex;
        }
        margin-bottom: 12px;
        align-items: center;
    }
    .role-name{
        width: 200px;
    }
    .item{
        width: 150px;
        margin-right: 12px;
    }
    .content{
        height: 100%;
        overflow: auto;
    }
    .required {
        color: red;
        margin-left: 4px;
    }
    .error-message {
        color: #ff4d4f;
        font-size: 12px;
        margin-top: 4px;
    }
    .input-wrapper {
        display: flex;
        flex-direction: column;
    }
}
</style>