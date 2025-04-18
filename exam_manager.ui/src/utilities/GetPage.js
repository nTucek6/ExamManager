import { useLocation, useParams } from "react-router-dom";
import { matchPath } from "react-router";

export default function GetPage() {
  const location = useLocation();

  const { id } = useParams();

  const isHome = !!matchPath(location.pathname, "/");
  const isExams = !!matchPath(location.pathname, "/exams");

  //Student pages -----------------------------------------------------------------------------

  const isExamsRegistration = !!matchPath(location.pathname, "/register-exam");
  const isAllExams = !!matchPath(location.pathname, "/all-exams");

  //Professor pages ---------------------------------------------------------------------------
  const isProfessorExams = !!matchPath(location.pathname, "/exams-professor");
  const isProfessorCreateExam = !!matchPath(location.pathname, "/create-exam");
  const isProfessorCreateExamWithParams = !!matchPath(
    location.pathname,
    "/create-exam/" + id
  );
  const isExamStudentWithParams = !!matchPath(
    location.pathname,
    "/exam-students/" + id
  );
  // ------------------------------------------------------------------------------------------

  if (isHome) {
    return;
  } else if (isExams) {
    return <h3>Pregled upisanih ispita</h3>;
  } else if (isProfessorExams) {
    return <h3>Pregled kreiranih ispitnih rokova</h3>;
  } else if (isProfessorCreateExam) {
    return <h3>Kreiranje ispitnog roka</h3>;
  } else if (isProfessorCreateExamWithParams) {
    return <h3>Ažuriranje ispitnog roka</h3>;
  } else if (isExamStudentWithParams) {
    return <h3>Ispis studenata za ispitni rok</h3>;
  } else if (isExamsRegistration) {
    return <h3>Prijava ispita</h3>;
  } else if (isAllExams) {
    return <h3>Pregled svih rokova</h3>;
  }
}
