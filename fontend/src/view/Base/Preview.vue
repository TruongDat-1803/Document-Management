
<template>
    <div>
        <cc-popup 
            v-model="value"
            :width="900"
            title="Thông tin thư mục"
            @close="close">
            <div class="preview">
                <VueDocPreview v-if="type == 'a'" :value="src" :type="type" />
                <iframe v-else class="iframe-preview"  :src="src"></iframe>
            </div>
        </cc-popup>
    </div>
</template>
<script>
import FileAPI from '@/api/Components/FileAPI.js';
import VueDocPreview from 'vue-doc-preview';

export default {
    components: {
        VueDocPreview
    },
    props: {
        value:{
            type: [String,Boolean],
            default: false
        },
        src: {
            type: String,
            default: null
        },
        name: {
            type: String,
            default: 'office'
        }
    },
    created(){
        // if(this.name && (this.name.includes('.docx') || this.name.includes('.pptx') || this.name.includes('.xlsx'))){
        //     this.type = 'office';
        // }
 
    },
    data(){
        return{
            loading: false,
            type: 'office'
        }
    },
    methods: {
        close(){
            this.$emit("input",false);
        },
    }
}
</script>
<style lang="scss" scoped>
.preview{
    height: 500px;
    .iframe-preview{
        width: 100%;
        height: 100%;
    }
}
</style>