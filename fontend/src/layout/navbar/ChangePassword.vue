<template>
    <div>
        <cc-popup 
            v-model="value"
            :width="500"
            title="Đổi mật khẩu"
            @close="close">
            <cc-row>
                <cc-col w="60">
                    <div class="line-input">
                            <label for="fullName" class="label-input">Mật khẩu cũ</label>
                            <cc-input v-model="oldPassword" :placeholderInput="'Nhập mật khẩu cũ'" />
                            <div v-if="errors.oldPassword" class="error-message">{{ errors.oldPassword }}</div>
                        </div>
                </cc-col>
            </cc-row>
            <cc-row>
                <cc-col w="60">
                    <div class="line-input">
                            <label for="fullName" class="label-input">Mật khẩu mới</label>
                            <cc-input v-model="newPassword" :placeholderInput="'Nhập mật khẩu mới'" />
                            <div v-if="errors.newPassword" class="error-message">{{ errors.newPassword }}</div>
                        </div>
                </cc-col>
            </cc-row>
            <cc-row>
                <cc-col w="60">
                    <div class="line-input">
                            <label for="fullName" class="label-input">Nhập lại mật khẩu mới</label>
                            <cc-input v-model="confirmPassword" :placeholderInput="'Nhập lại mật khẩu mới'" />
                            <div v-if="errors.confirmPassword" class="error-message">{{ errors.confirmPassword }}</div>
                        </div>
                </cc-col>
            </cc-row>
            <div class="footer">
                <cc-button type="primary" @click="handleChangePassword" :loading="loading">
                    Xác nhận
                </cc-button>
            </div>
        </cc-popup>
    </div>
</template>
<script>
export default {
    props: {
        value:{
            type: [String,Boolean],
            default: false
        }
    },
    data(){
        return{
            loading: false,
            oldPassword: '',
            newPassword: '',
            confirmPassword: '',
            errors: {
                oldPassword: '',
                newPassword: '',
                confirmPassword: ''
            }
        }
    },
    methods: {
        validateForm() {
            let isValid = true;
            
            return isValid;
        },
        validatePassword(password) {
            if (password.length < 8) {
                return false;
            }
            const hasLetter = /[a-zA-Z]/.test(password);
            const hasNumber = /[0-9]/.test(password);
            return hasLetter && hasNumber;
        },
        async handleChangePassword() {
            this.errors.oldPassword = '';
            this.errors.newPassword = '';
            this.errors.confirmPassword = '';
            if (!this.oldPassword) {
                this.errors.oldPassword = 'Vui lòng nhập mật khẩu cũ';
                return;
            }
            if (!this.newPassword) {
                this.errors.newPassword = 'Vui lòng nhập mật khẩu mới';
                return;
            }
            if (!this.validatePassword(this.newPassword)) {
                this.errors.newPassword = 'Mật khẩu phải có ít nhất 8 ký tự và bao gồm cả chữ và số';
                return;
            }
            if (!this.confirmPassword) {
                this.errors.confirmPassword = 'Vui lòng nhập lại mật khẩu';
                return;
            }
            if (this.newPassword !== this.confirmPassword) {
                this.errors.confirmPassword = 'Mật khẩu nhập lại không khớp';
                return;
            }
            this.$store.dispatch("common/setPassword", this.newPassword)
            var param = {
                EmplyeeID: this.$store.getters.employeeID,
                Password: this.newPassword
            };
            console.log(param);
            debugger;
            this.loading = true;
            try {
                // TODO: Gọi API đổi mật khẩu ở đây
                // const response = await this.$api.changePassword({
                //     oldPassword: this.oldPassword,
                //     newPassword: this.newPassword
                // });
                
                this.$emit('close');
                this.$message.success('Đổi mật khẩu thành công');
                this.close();
            } catch (error) {
                this.$message.error('Đổi mật khẩu thất bại');
            } finally {
                this.loading = false;
            }
        },

        close() {
            this.$emit('input', false);
            this.resetForm();
        },

        resetForm() {
            this.oldPassword = '';
            this.newPassword = '';
            this.confirmPassword = '';
            this.errors = {
                oldPassword: '',
                newPassword: '',
                confirmPassword: ''
            };
        }
    }
}
</script>
<style lang="scss" scoped>
.footer{
    float: right;
    margin-top: 20px;
}
.line-input {
    width: 100%;
            display: flex;
            flex-direction: column;
        }
        .line-tool {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            margin-top: 0.5rem/* 8px */;
            margin-bottom: 0.5rem/* 8px */;
        }
            .label-input {
                margin-bottom: 10px;
            } 
            .item-tool {
                text-decoration: underline;
                font-style: italic;
                cursor: pointer;
            }
        .line-submit {
            display: flex;
            margin-top: 2.5rem/* 40px */;
            margin-bottom: 2.5rem/* 40px */;
            justify-content: center;
        }
        .error-message {
            color: #ff4d4f;
            font-size: 12px;
            margin-top: 4px;
        }
</style>