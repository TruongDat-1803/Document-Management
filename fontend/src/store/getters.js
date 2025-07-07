const getters = {
    tenantID: state => state.common.tenantID,
    userID: state => state.common.userID,
    employeeID: state => state.common.employeeID,
    token: state => state.common.token,
    listRole: state => state.common.listRole,
    ShareIDs: state => state.common.ShareIDs,
    verifyCode: state => state.common.verifyCode,
    username: state => state.common.username,
    password: state => state.common.password,
}
export default getters