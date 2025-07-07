
<template>
    <div>
        <cc-popup 
            v-model="value"
            :width="500"
            title="Thông tin thư mục"
            @close="close">
            <div class="image-preview-container">
                <!-- <iframe :src="src" alt="Ảnh" class="image-preview-img"> -->
                    <iframe  class="iframe-preview"  :src="src"></iframe>
            </div>
            
        </cc-popup>
    </div>
</template>
<script>
import FileAPI from '@/api/Components/FileAPI.js';
export default {
    props: {
        value:{
            type: [String,Boolean],
            default: false
        },
        src: {
            type: String,
            default: null
        }
    },
    data(){
        return{
            loading: false
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
.image-preview-container {
  height: 600px; /* hoặc chiều cao mong muốn */
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: auto;
}
.image-preview-img {
  max-width: 100%;
  max-height: 100%;
  display: block;
  margin: auto;
  object-fit: contain; /* Giữ tỉ lệ, không bị méo */
}
</style>