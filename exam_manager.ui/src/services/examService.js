import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const examService = {
  getStudentExams: async (StudentId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/Exam/GetStudentExams`,
        headers: { "Content-Type": "application/json" },
        params: {
          StudentId: StudentId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
};

export default examService;
