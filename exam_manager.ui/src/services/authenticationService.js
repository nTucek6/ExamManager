import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const authenticationService = {
  login: async (Email, Password) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/Authentication/Login`,
        headers: { "Content-Type": "application/json" },
        data: {
          Email: Email,
          Password: Password,
        },
      });

      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
      //throw error.response?.data || "Login failed";
    }
  },
  register: async (Email, Password, Name, Surname) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/Authentication/Register`,
        headers: { "Content-Type": "application/json" },
        data: {
          Email: Email,
          Password: Password,
          Name: Name,
          Surname: Surname,
        },
      });

      const data = response;
      return data;
    } catch (error) {
      throw error.response?.data || "Register failed";
    }
  },
    changePassword: async (oldPassword, newPassword) => {
        try {
            const token = sessionStorage.getItem("token");
            const response = await axios.post(
                `${API_URL}/Authentication/ChangePassword`,
                { oldPassword, newPassword },
                {
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`
                    }
                }
            );
            return response;
        } catch (error) {
            throw error;
        }
    },

};

export default authenticationService;
