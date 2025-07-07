const state = {
    tenantID: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    userID: "",
    employeeID: 1,
    token: null,
    listRole: null,
    ShareIDs: null,
    verifyCode: null,
    username: null,
    password: null
  }
  
  const mutations = {
    setTenantID(state,val){
      state.tenantID = val;
    },
    setToken(state,val){
      state.token = val;
    },
    setListRole(state,val){
      state.listRole = val;
    },
    setEmployeeID(state,val){
      state.employeeID = val;
    },
    setShareIDs(state,val){
      state.ShareIDs = val;
    },
    setUsername(state,val){
      state.username = val;
    },
    setPassword(state,val){
      state.password = val;
    },
    setVerifyCode(state,val){
      state.verifyCode = val;
    }
  }
  
  const actions = {
    setTenantID({ commit }, val) {
      commit('setTenantID',val)
    },
    setToken({ commit },val){
      commit('setToken',val)
    },
    setListRole({ commit },val){
      commit('setListRole',val)
    },
    setEmployeeID({ commit },val){
      commit('setEmployeeID',val)
    },
    setShareIDs({ commit },val){
      commit('setShareIDs',val)
    },
    setUsername({ commit },val){
      commit('setUsername',val)
    },
    setPassword({ commit },val){
      commit('setPassword',val)
    },
    setVerifyCode({ commit },val){
      commit('setVerifyCode',val)
    }
  }
  
  export default {
    namespaced: true,
    state,
    mutations,
    actions
  }