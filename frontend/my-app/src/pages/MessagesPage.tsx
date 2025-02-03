import { useState} from "react"
import "../styles/swipe.css"
import Sidebar from "../components/Sidebar";
import MessageProfileCard from "../components/MatchProfileCard";
import { CircularProgress } from "@mui/material";
import ChatHome from "../ChatHome";
import { useParams, useLocation } from "react-router-dom";
export default function MessagePage() {
    const { matchId } = useParams<{ matchId: string }>();
    const [loading, setLoading] = useState(false);
    const location = useLocation();
    const matchData = location.state?.matchData;
    console.log('message, ', matchData)
    return(
        <>
        <div className="container">
            <div className="sidebar-container">
                <Sidebar isUserPreferences={false}/>
            </div>
            { loading ? (
                <CircularProgress sx={{ color: "#864c4c", margin: 'auto auto' }} size={48} />
            ) : (
                <div className="message-container">
                    <div className="message-chat-room-container">
                        { matchId && <ChatHome 
                                        userName="test" 
                                        chatRoom={matchId} 
                                        imageData={matchData.imageData} 
                                        imageFormat={matchData.imageFormat}
                                        profileName={matchData.name}
                                        createdAt={matchData.createdAt}/> }
                    </div>
                    <div className="profile-match-container">
                        <MessageProfileCard matchData={matchData}/>
                    </div>
                </div>
            )}
            
        </div>
            
        </>);
}