import { useEffect, useState} from "react";
import ConfirmedWindow from "../components/ConfirmedWindow";
import { CircularProgress } from "@mui/material";
import { useNavigate, useParams } from 'react-router-dom';
import { confirmAccount, ConfirmAccountRequest } from "../api/authApi";
export default function ConfirmedAccountPage() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState<boolean>(false);
    const { token } = useParams<{ token: string }>();
    useEffect(() => {
        if (token) {
            setLoading(true);
            const confirmAccountRequest: ConfirmAccountRequest = { token };
            confirmAccount(confirmAccountRequest)
                .then(() => {
                    console.log('confirmed');
                })
                .catch((err) => {
                    console.error(err);
                })
                .finally(() => {
                    setLoading(false);
                });
        }
    }, [token]); 
    
    return(
    <div style={{display:'flex', justifyContent:'center', alignItems:'center', flexDirection:'column', margin:'10%'}}>
        {loading ? (
            <CircularProgress sx={{color: "#864c4c" }} size={32} />): (<>
        <ConfirmedWindow title="Your account has been confirmed!" message="Click a button below to go to login"/>
        <button className="load__button" type='submit' onClick={() => navigate("/login")}>
            Back to login page
        </button></>)}
    </div>)
}