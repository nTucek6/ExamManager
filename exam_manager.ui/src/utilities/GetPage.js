import { useLocation,useParams } from "react-router-dom";
import { matchPath } from "react-router";

export default function GetPage() {
    const location = useLocation();

    const isHome = !!matchPath(location.pathname, '/');
    const isExams = !!matchPath(location.pathname, '/exams');

    if (isHome) {
        return <h3>Početna</h3>;
    }
    else if(isExams)
    {
        return <h3>Pregled upisanih ispita</h3>;
    }

}