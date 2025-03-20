import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const authenticationService = {
  login: async (Email, Password) => {
    try {
      const response = await axios.post(`${API_URL}/Authentication/Login`, {
        Email,
        Password,
      });
      const data = response.data;
      return data;
    } catch (error) {
      throw error.response?.data || "Login failed";
    }
  },
  register: async (Email, Password, ConfirmPassword) => {
    try {
      const response = await axios.post(`${API_URL}/Authentication/Register`, {
        Email,
        Password,
        ConfirmPassword,
      });
      const data = response.data;
      return data;
    } catch (error) {
      throw error.response?.data || "Register failed";
    }
  },

};

export default authenticationService;
