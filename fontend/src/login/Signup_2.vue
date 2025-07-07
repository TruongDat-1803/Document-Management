<template>
    <div>
        <div class="page-login">
            <div class="box-login">
                <div class="title-login">
                    <img src="../assets/icon/sidebar/logo-2.png" width="100" height="100" alt="">
                    <div style="margin-top: 20px">
                        DataLink
                    </div>
                </div>
                <form @submit.prevent>
                    <div v-if="!showRegistrationForm" class="line-input">
                        <label for="" class="label-input">Nhập mã xác thực</label>
                        <cc-input v-model="checkcode" :placeholderInput="placeholderCode" />
                        <div v-if="errorCode" class="error-message">{{ errorCode }}</div>
                    </div>
                    <div v-if="showRegistrationForm">
                        <div class="line-input">
                            <label for="fullName" class="label-input">Họ và tên</label>
                            <cc-input v-model="fullName" :placeholderInput="'Nhập họ và tên'" />
                            <div v-if="errorFullName" class="error-message">{{ errorFullName }}</div>
                        </div>
                        <div class="line-input">
                            <label for="phone" class="label-input">Số điện thoại</label>
                            <cc-input v-model="phone" :placeholderInput="'Nhập số điện thoại'" />
                            <div v-if="errorPhone" class="error-message">{{ errorPhone }}</div>
                        </div>
                        <div class="dob-gender-container">
                            <div class="line-input">
                                <label for="dob" class="label-input">Ngày sinh</label>
                                <div class="date-input-wrapper">
                                    <input type="date" id="dob" v-model="dateOfBirth" class="date-input" />
                                </div>
                                <div v-if="errorDob" class="error-message">{{ errorDob }}</div>
                            </div>
                            <div class="line-input">
                                <label for="gender" class="label-input">Giới tính</label>
                                <div class="gender-input-wrapper">
                                    <select v-model="gender" class="gender-input">
                                        <option value="">Chọn giới tính</option>
                                        <option value="male">Nam</option>
                                        <option value="female">Nữ</option>
                                        <option value="other">Khác</option>
                                    </select>
                                </div>
                                <div v-if="errorGender" class="error-message">{{ errorGender }}</div>
                            </div>
                        </div>
                        <div class="line-input">
                            <label for="company" class="label-input">Tên công ty</label>
                            <cc-input v-model="company" :placeholderInput="'Nhập tên công ty'" />
                            <div v-if="errorCompany" class="error-message">{{ errorCompany }}</div>
                        </div>
                        <div class="line-input">
                            <label for="position" class="label-input">Chức vụ</label>
                            <cc-input v-model="position" :placeholderInput="'Nhập chức vụ'" />
                            <div v-if="errorPosition" class="error-message">{{ errorPosition }}</div>
                        </div>
                    </div>
                    <div class="line-tool">
                        <div class="item-tool"><RouterLink to="/login">Quay lại đăng nhập</RouterLink></div>
                    </div>
                    <div class="line-submit">
                        <button type="button" class="btn-common-primary" @click="showRegistrationForm ? signup_2() : ValidateSignup()">
                            {{ showRegistrationForm ? 'Hoàn tất đăng ký' : 'Tiếp tục đăng ký' }}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>

</template>
<script>
import UserAPI from '../api/Components/UserAPI';
import ccInput from '../components/input/ccInput.vue'
export default {
    components: { ccInput },
    data() {
        return {
            placeholderCode: "Mã xác thực gồm 6 chữ số",
            checkcode: "",
            errorCode: "",
            showRegistrationForm: false,
            fullName: "",
            phone: "",
            dateOfBirth: "",
            gender: "",
            company: "",
            position: "",
            errorFullName: "",
            errorPhone: "",
            errorDob: "",
            errorGender: "",
            errorCompany: "",
            errorPosition: ""
        }
    },
    computed: {
        username() {
            return this.$store.getters.username;
        },
        password() {
            return this.$store.getters.password;
        }
    },
    methods: {
        validatePhone(phone) {
            if (phone.startsWith('0')) {
                return /^0[0-9]{9}$/.test(phone);
            } else if (phone.startsWith('84')) {
                return /^84[0-9]{9}$/.test(phone);
            }
            return false;
        },
        calculateAge(birthDate) {
            const today = new Date();
            const birth = new Date(birthDate);
        
            if (birth.getFullYear() > 2025 || birth.getFullYear() < 1925) {
                return -1; 
            }
            let age = today.getFullYear() - birth.getFullYear();
            const monthDiff = today.getMonth() - birth.getMonth();
            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
                age--;
            }
            return age;
        },
        ValidateSignup(){
            if (!this.showRegistrationForm) {
                this.errorCode = "";
                if (!this.checkcode) {
                    this.errorCode = "Vui lòng nhập mã xác thực";
                    return;
                }
                if (this.checkcode !== this.$store.getters.verifyCode) {
                    this.errorCode = "Mã xác thực không đúng";
                    return;
                }
                this.showRegistrationForm = true;
            }
        },
        signup_2(){
             if (this.showRegistrationForm){
                this.errorFullName = "";
                this.errorPhone = "";
                this.errorDob = "";
                this.errorGender = "";
                this.errorCompany = "";
                this.errorPosition = "";
                if (!this.fullName) {
                    this.errorFullName = "Vui lòng nhập họ và tên";
                    // return;
                }
                if (!this.phone) {
                    this.errorPhone = "Vui lòng nhập số điện thoại";
                    //return;
                }
                if (!this.validatePhone(this.phone)) {
                    this.errorPhone = "Số điện thoại không hợp lệ. Phải bắt đầu bằng 0 (10 số) hoặc 84 (11 số)";
                    //return;
                }
                if (!this.dateOfBirth) {
                    this.errorDob = "Vui lòng chọn ngày sinh";
                    //return;
                }
                const age = this.calculateAge(this.dateOfBirth);
                if (age === -1) {
                    this.errorDob = "Vui lòng không đùa";
                    this.errorDob = "Năm sinh không hợp lệ";
                    //return;
                }
                if (age < 16) {
                    this.errorDob = "Bạn phải đủ 16 tuổi để đăng ký";
                    //return;
                }
                if (!this.gender) {
                    this.errorGender = "Vui lòng chọn giới tính";
                    //return;
                }
                if (!this.company) {
                    this.errorCompany = "Vui lòng nhập tên công ty";
                    //return;
                }
                if (!this.position) {
                    this.errorPosition = "Vui lòng nhập chức vụ";
                    return;
                }
            }
            var param ={
                username: this.username,
                password: this.password,
                fullName: this.fullName,
                phone: this.phone,
                dateOfBirth: this.dateOfBirth,
                gender: this.gender,
                company: this.company,
                position: this.position
            }
            UserAPI.Signup(param).then(async res => 
            {
                if(res.data.Success){
                    var param1 = {
                        Username: this.username,
                        Password: this.password
                    };
                    var me = this;
                    UserAPI.Login(param1).then(async res => {
                    if(res.data){
                        localStorage.setItem('token', res.data.Token);
                        me.$store.dispatch("common/setToken", res.data.Token);
                        await me.GetUserInfo();
                        me.$router.push("/home");
                    }
            });
                }
            });
        },
        async GetUserInfo(){
            var res = await UserAPI.GetUserInfo();
            if(res.data && res.data.Success){
                var listRole = res.data.Data.ListRole;
                if(listRole){
                    this.$store.dispatch('common/setListRole',listRole);
                }
            }
        },
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
        .dob-gender-container {
            display: flex;
            gap: 20px;
            margin-top: 1.25rem;
            margin-bottom: 1.25rem;
        }
        .dob-gender-container .line-input {
            flex: 1;
            margin: 0;
        }
        .date-input-wrapper {
            width: 80%;
            .date-input {
                width: 100%;
                height: 36px;
                padding: 0 16px;
                border: 1px solid #d9d9d9;
                border-radius: 4px;
                font-size: 14px;
                outline: none;
                &:focus {
                    border-color: #2962FF;
                }
            }
        }
        .gender-input-wrapper {
            width: 100%;
            .gender-input {
                width: 100%;
                height: 36px;
                padding: 0 16px;
                border: 1px solid #d9d9d9;
                border-radius: 4px;
                font-size: 14px;
                outline: none;
                background-color: white;
                cursor: pointer;
                &:focus {
                    border-color: #2962FF;
                }
            }
        }
</style>