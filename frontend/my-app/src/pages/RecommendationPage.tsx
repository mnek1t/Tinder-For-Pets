import "../styles/swipe.css"
import {useEffect, useState} from "react"
import Sidebar from "../components/Sidebar";
import RecommendationCardSwipe from "../components/RecommendationCardSwipe";
import { getProfileRecommendations, createSwipe } from "../api/profileApi"; 
import { CircularProgress } from "@mui/material";
import NoData from "../components/NoData";
import heartBrokenImg from '../assets/heart-broken-svgrepo-com.svg'
export type RecomendationCard =  {
    id: string
    name: string,
    age: number,
    imageData: string,
    imageFormat: string
}
export default function RecommendationPage() {
    const [loading, setLoading] = useState(false);
    const [recommendationCards, setRecommendationCards] = useState<RecomendationCard[]>([])
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        setLoading(true);
        getProfileRecommendations()
        .then(data => {
            const cards : RecomendationCard[] = data.map(card => ({
                id: card.profile.id,
                name : card.profile.name,
                age : card.profile.age,
                imageData: card.images[0].imageData,
                imageFormat: card.images[0].imageFormat,
            })) 
            setRecommendationCards(cards);
        })
        .catch((error) => console.error(error))
        .finally(() => setLoading(false));
        
        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, []);

    function handleSwipe(profileId: string, isLike: boolean) {
        createSwipe(profileId, isLike)
    }

    return(
        <>
        <div className="container">
            <div className="sidebar-container">
                <Sidebar isUserPreferences={false}/>
            </div>
            { loading ? (
                <CircularProgress sx={{ color: "#864c4c", margin: 'auto auto' }} size={48} />
            ) : (
                recommendationCards.length > 0 ? (
                    <div className="swiping-profiles">
                        <RecommendationCardSwipe 
                            key={recommendationCards[0].id} 
                            {...recommendationCards[0]}  
                            cards={recommendationCards} 
                            setCards={setRecommendationCards} 
                            handleSwipe={handleSwipe}
                        />
                    </div>
                ) : (
                    <div className="no-data-container">
                        <NoData imageSrc={heartBrokenImg} title="No recommendations!" description="Try to adjust ditance preferences in you profile page or come back later!"/>
                    </div>
                )
            )}
            
        </div>
            
        </>
    );
}