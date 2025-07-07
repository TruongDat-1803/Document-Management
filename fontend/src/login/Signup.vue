<template>
    <div>
        <div class="page-login" v-if="!showSignup_2">
            <div class="box-login">
                <div class="title-login">
                    <img src="../assets/icon/sidebar/logo-2.png" width="100" height="100" alt="">
                    <div style="margin-top: 20px">
                        DataLink
                    </div>
                </div>
                <form action="">
                    <div class="line-input">
                        <label for="" class="label-input">Nhập email của bạn</label>
                        <cc-input v-model="username" :placeholderInput="placeholderUsername" />
                        <div v-if="errorUsername" class="error-message">{{ errorUsername }}</div>
                    </div>
                    <div class="line-input">
                        <label for="" class="label-input">Mật khẩu</label>
                        <cc-input v-model="password" :mode="mode1" :placeholderInput="placeholderPassword" :iconTails="icon1" @clickIcon="clickIcon(0)" />
                        <div v-if="errorPassword" class="error-message">{{ errorPassword }}</div>
                    </div>
                    <div class="line-input">
                        <label for="" class="label-input">Nhập lại mật khẩu</label>
                        <cc-input v-model="rePassword" :mode="mode2" :placeholderInput="placeholderPassword" :iconTails="icon2" @clickIcon="clickIcon(1)" />
                        <div v-if="errorRePassword" class="error-message">{{ errorRePassword }}</div>
                    </div>
                    <div class="line-tool">
                        <div class="item-tool"><RouterLink to="/forgot-password">Quên mật khẩu</RouterLink></div>
                        <div class="item-tool"><RouterLink to="/login">Đăng nhập</RouterLink></div>
                    </div>
                    <div class="line-submit">
                        <button class="btn-common-primary" @click="signup">Đăng ký</button>
                    </div>
                </form>
            </div>
        </div>
        <signup_2 v-if="showSignup_2" :username="username" :password="password" />
    </div>
</template>
<script>
import ccInput from '../components/input/ccInput.vue'
import signup_2 from './Signup_2.vue'
import EmailAPI from '../api/Components/EmailAPI'
export default {
    components: { ccInput, signup_2 },
    data() {
        return {
            placeholderUsername:"Email hoặc Số điện thoại",
            placeholderPassword: "Mật khẩu",
            placeholderRePassword: "Nhập lại mật khẩu",
            typeInput1 : false,
            typeInput2: false,
            mode1: 'password',
            mode2: 'password',
            icon1: 'icon-eye-hide',
            icon2: 'icon-eye-hide',
            username: "",
            password: "",
            rePassword: "",
            errorUsername: "",
            errorPassword: "",
            errorRePassword: "",
            showSignup_2: false
        }
    },
    methods: {
        validateEmail(email) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailRegex.test(email);
        },
        async signup(){
            this.errorUsername = '';
            this.errorPassword = '';
            this.errorRePassword = '';
            if (!this.username) {
                this.errorUsername = 'Vui lòng nhập email';
                //return;
            }
            if (!this.validateEmail(this.username)) {
                this.errorUsername = 'Email không hợp lệ';
                //return;
            }
            if (!this.password) {
                this.errorPassword = 'Vui lòng nhập mật khẩu';
                //return;
            }
            if (!this.validatePassword(this.password)) {
                this.errorPassword = 'Mật khẩu phải có ít nhất 8 ký tự và bao gồm cả chữ và số';
                //return;
            }
            if (!this.rePassword) {
                this.errorRePassword = 'Vui lòng nhập lại mật khẩu';
                //return;
            }
            if (this.password !== this.rePassword) {
                this.errorRePassword = 'Mật khẩu nhập lại không khớp';
                return;
            }
            this.$store.dispatch("common/setUsername", this.username);
            this.$store.dispatch("common/setPassword", this.password);
            var param = {
                Email: this.username,
                Password: this.password
            };
            var res = await EmailAPI.VerifyEmail(param);
            if(res.data.Success){
                this.showSignup_2 = true;
                const verifyCode = res.data.Data || res.data.verifyCode; 
                this.$store.dispatch("common/setVerifyCode", verifyCode);
            } 
            else 
            {
                this.errorUsername = res.data?.Message || 'Có lỗi xảy ra khi gửi mã xác thực';
            }
        },
        clickIcon(val) {
            switch(val) {
                case 0: 
                    this.typeInput1 = !this.typeInput1;
                    if(this.typeInput1) {
                        this.mode1 = 'text';
                        this.icon1 = 'icon-eye-show';
                    } else {
                        this.mode1 = 'password';
                        this.icon1 = 'icon-eye-hide';
                    }
                break;
                case 1: 
                     this.typeInput2 = !this.typeInput2;
                    if(this.typeInput2) {
                        this.mode2 = 'text';
                        this.icon2 = 'icon-eye-show';
                    } else {
                        this.mode2 = 'password';
                        this.icon2 = 'icon-eye-hide';
                    }
            }
        },
        validatePassword(password) {
            if (password.length < 8) {
                return false;
            }
            const hasLetter = /[a-zA-Z]/.test(password);
            const hasNumber = /[0-9]/.test(password);
            return hasLetter && hasNumber;
        }
    },
}
</script>
<style lang="scss" scoped>
.page-login {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100vw;
    height: 100vh;
    background-image: url('../assets/image/banner1.jpg');
    background-size: cover;
}
    .box-login {
        display: flex;
        flex-direction: column;
        width: 450px;
        background-color: #ffffff;
        border-radius: 10px;
        padding: 25px;
    }
        .title-login {
            width: 100%;
            text-align: center;
            font-weight: 700;
            font-size: 3rem/* 48px */;
            line-height: 1;
            color: #2962FF;
            margin-bottom: 2rem;
        }
        .line-input {
            display: flex;
            margin-top: 1.25rem/* 20px */;
            margin-bottom: 1.25rem/* 20px */;
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
        .btn-common-primary {
            position: relative;
            height: 36px;
            padding: 0 16px;
            display: flex;
            align-items: center;
            justify-content: center; 
            border-radius: 4px;
            font-size: 14px;
            font-weight: 500;
            outline: none;
            min-width: 80px;
            width: 100%;
            cursor: pointer;
            color: #ffffff;
            background-color: #2962FF;
            border: none;
            &:hover{
                background-color: #2979FF;
            }
            &:active{
                background-color: #304FFE;
            }
        }
        .error-message {
            color: #ff4d4f;
            font-size: 12px;
            margin-top: 4px;
        }
</style>