<template>
    <div>

    </div>
</template>
<script>
export default {
    methods: {
        checkPermission(subCode, action, me){
            // Lấy danh sách quyền từ Vuex store
            var listRole = me.$store.getters.listRole;
    
            // Kiểm tra xem danh sách quyền có tồn tại không
            if(listRole){
                // Tìm quyền cụ thể trong danh sách dựa trên SubCode và Action
                var item = listRole.find(x => x.SubCode == subCode && x.Action == action);
                console.log('Found Permission:', item);
                // Trả về giá trị Value của quyền tìm thấy
                if(item){
                    return item.Value;
                }
                return false;
            }
    // Nếu không có danh sách quyền, trả về false
    console.log('Found Permission:', item);

    return false;
}
    }
}
</script>
<style lang="scss" scoped>

</style>