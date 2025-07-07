<template>
    <div>
        <cc-popup 
            v-model="value"
            :width="500"
            title="Thêm người dùng"
            @close="close">
            <cc-row>
                <cc-col w="40">
                    Tên người dùng
                </cc-col>
                <cc-col w="60">
                    <cc-select-box v-model="user.EmployeeID"
                        :dataSource="listEmployee"
                        displayField="EmployeeName"
                        valueField="EmployeeID"
                        @selected="selectEmployee"></cc-select-box>
                </cc-col>
            </cc-row>
            <cc-row>
                <cc-col w="40">
                    Vai trò
                </cc-col>
                <cc-col w="60">
                    <cc-select-box v-model="user.RoleID"
                        :dataSource="listRole"
                        displayField="RoleName"
                        valueField="RoleID"
                        @selected="selectRole"></cc-select-box>
                </cc-col>
            </cc-row>
            <cc-row>
                <cc-col w="40">
                    Quyền truy cập dữ liệu
                </cc-col>
                <cc-col w="60">
                    <cc-organization-unit v-model="user.OrganizationUnitID"></cc-organization-unit>
                </cc-col>
            </cc-row>
            <div class="footer flex">
                <cc-button type="secondary-border" class="m-r-12" @click="close">
                    Hủy
                </cc-button>
                <cc-button type="primary" @click="save" :loading="loading">
                    Lưu
                </cc-button>
            </div>
        </cc-popup>
    </div>
</template>
<script>
import UserAPI from '@/api/Components/UserAPI.js' ;
import EmployeeAPI from '../../../api/Components/EmployeeAPI';
import RoleAPI from '@/api/Components/RoleAPI.js';
export default {
    props: {
        value:{
            type: [String,Boolean],
            default: false
        },
        listFolder: {
            type: Array,
            default: () => []
        }
    },
    data(){
        return{
            user: {
                EmployeeName: null,
                EmployeeID: null,
                OrganizationUnitID: null,
                OrganizationUnitName: null,
                Email: null,
                Phone: null,
                RoleID: null,
                RoleName: null,
                Password: '12345678@Abc'
            },
            loading: false,
            listEmployee : [],
            listRole: []
        }
    },
    methods: {
        selectEmployee(val){
            this.user.EmployeeID = val.EmployeeID;
            this.user.EmployeeName = val.EmployeeName;
            this.user.Email = val.Email;
            this.user.Phone = val.Phone;
            this.user.OrganizationUnitID = parseInt(val.OrganizationUnitID);
            this.user.OrganizationUnitName = val.OrganizationUnitName;
            this.user.RoleID = val.RoleID;
            this.user.RoleName = val.RoleName;
            this.user.TenantID = val.TenantID;
        },
        selectRole(val){
            this.user.RoleID = val.RoleID;
            this.user.RoleName = val.RoleName;
        },
        GetRole(){
            var me = this;
            RoleAPI.GetAll().then(res => {
                if(res.data && res.data.Success){
                    me.listRole = res.data.Data;
                }
            });
        },
        close(){
            this.$emit("input",false);
        },
        save(){
            this.loading = true;
            var me = this;
            console.log(this.user);
            UserAPI.InsertUser(this.user).then(res => {
                if(res.data && res.data.Success){
                    this.$message.success('Thêm người dùng thành công');
                    this.$emit("save",false);
                } else {
                    this.$message.error('Thêm người dùng thất bại');
                }
                this.loading = false;
            }).catch(error => {
                this.$message.error('Thêm người dùng thất bại');
                this.loading = false;
            });
        },
        getAllEmployee(){
            var me = this;
            EmployeeAPI.GetAll().then(res => {
                if(res.data && res.data.Success){
                    me.listEmployee = res.data.Data;
                }
            })
        },
    },
    created() {
        this.GetRole();
        this.getAllEmployee();
    }
}
</script>
<style lang="scss" scoped>
.chose-file{
    height: 300px;
    display: flex;
    align-items: center;
    justify-content: center;
}
.footer{
    float: right;
}
</style>