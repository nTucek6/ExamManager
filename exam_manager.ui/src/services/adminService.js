import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
const ADMIN_ACTIONS_CONTROLLER = process.env.REACT_APP_API_ADMIN_ACTIONS_CONTROLLER;

const adminService = {
  getUserRoles: async () => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${ADMIN_ACTIONS_CONTROLLER}/GetUserRoles`,
        headers: { "Content-Type": "application/json" },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  getUser: async () => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${ADMIN_ACTIONS_CONTROLLER}/GetUsers`,
        headers: { "Content-Type": "application/json" },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  changeUserRole: async (UserId, RoleId) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/${ADMIN_ACTIONS_CONTROLLER}/ChangeUserRole`,
        headers: { "Content-Type": "application/json" },
        data: {
          UserId: UserId,
          UserRole: RoleId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
};

export default adminService;
