import { useEffect, useState } from "react";
import { format } from "date-fns";
import { ClipLoader } from "react-spinners";
import "./ProfessorExams.css";

import examService from "../../../services/examService";
import { useNavigate } from "react-router";

export default function ProfessorExams() {
  const navigate = useNavigate();
  const [professorExams, setProfessoerExams] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    const result = await examService.getProfesorExams(
      Number(sessionStorage.getItem("userId"))
    );
    setProfessoerExams(result.data);
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleEdit = (id) => {
    navigate(`/create-exam/${id}`);
  };

  const handleShowStudents = (id) => {
     navigate(`/exam-students/${id}`);
  }

  const handleDelete = async (id) => {
   await examService.deleteExamPeriod(id);
   fetchData();
  }

  return (
    <div id="exam-container">
      {loading ? (
        <ClipLoader />
      ) : professorExams.length > 0 ? (
        professorExams.map((exam) => {
          return (
            <ul key={exam.ExamId}>
              <li>{exam.SubjectName}</li>
              <li>Lokacija: {exam.ExamLocation}</li>
              <li>
                Datum roka:{" "}
                {format(new Date(exam.DeadlineDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
              <li>
                Prijave do:{" "}
                {format(new Date(exam.ApplicationsDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
              <li>
                Odjave do:{" "}
                {format(new Date(exam.CheckOutDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
              <button type="button" onClick={()=>handleShowStudents(exam.ExamId)}>Prikaz upisanih studenta</button>
              <button type="button" onClick={()=> handleEdit(exam.ExamId)}>Uredi ispitni rok</button>
              <button type="button" onClick={()=> handleDelete(exam.ExamId)}>Obriši</button>
            </ul>
          );
        })
      ) : (
        <h1>Profesor trenutno nije kreirao ispitni rok!</h1>
      )}
    </div>
  );
}
