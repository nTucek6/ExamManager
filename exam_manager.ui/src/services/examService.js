import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const PROFESSOR_CONTROLLER = process.env.REACT_APP_API_PROFESSOR_CONTROLLER;

const examService = {
  getProfesorExams: async (ProfessorId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/GetProfessorExams`,
        headers: { "Content-Type": "application/json" },
        params: {
          ProfessorId: ProfessorId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  getProfesorSubjects: async (ProfessorId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/GetProfessorSubjects`,
        headers: { "Content-Type": "application/json" },
        params: {
          ProfessorId: ProfessorId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  getProfessorExamPeriod: async (ExamId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/GetExamPeriod`,
        headers: { "Content-Type": "application/json" },
        params: {
          ExamId: ExamId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  createExamPeriod: async (newExamDTO) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/CreateExamPeriod`,
        headers: { "Content-Type": "application/json" },
        data: newExamDTO,
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  updateExam: async (exam) => {
    try {
      const response = await axios({
        method: "post",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/UpdateExamPeriod`,
        headers: { "Content-Type": "application/json" },
        data: exam,
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  deleteExamPeriod: async (examId) => {
    try {
      const response = await axios({
        method: "delete",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/DeleteExamPeriod`,
        headers: { "Content-Type": "application/json" },
        params: {
          examId: examId,
        },
      });
      const data = response;
      return data;
    } catch (error) {
      return error.response.data;
    }
  },
  getProfessorExamStudents: async (ExamId) => {
    try {
      const response = await axios({
        method: "get",
        url: `${API_URL}/${PROFESSOR_CONTROLLER}/GetProfessorExamStudents`,
        headers: { "Content-Type": "application/json" },
        params: {
          ExamId: ExamId,
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
