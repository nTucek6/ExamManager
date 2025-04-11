import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
const STUDENT_CONTROLLER = process.env.REACT_APP_API_STUDENT_CONTROLLER;

const studentExamService = {
  getStudentExams: async (StudentId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${STUDENT_CONTROLLER}/GetStudentExams`,
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
  registerStudentExam: async (StudentId, ExamId) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/${STUDENT_CONTROLLER}/RegisterStudentExam`,
        headers: { "Content-Type": "application/json" },
        data: {
          ExamId: ExamId,
          StudentId: StudentId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  deleteStudentExam: async (StudentId, ExamId) => {
    try {
      const response = await axios({
        method: "delete",
        url: `${API_URL}/${STUDENT_CONTROLLER}/DeleteStudentExam`,
        headers: { "Content-Type": "application/json" },
        params: {
          ExamId: ExamId,
          StudentId:StudentId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },

  getExamsForRegister: async (StudentId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${STUDENT_CONTROLLER}/GetExamsForRegister`,
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
  
  getAllStudentExams: async (StudentId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${STUDENT_CONTROLLER}/GetAllStudentExams`,
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

export default studentExamService;
