import React, { useEffect, useState } from "react";
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Box from "@mui/material/Box"
import MatchCard from "./MatchCard";
import { getMatches } from "../api/profileApi";
import { CircularProgress } from "@mui/material";
import messagesImg from '../assets/messages-2-svgrepo-com.svg'
import matchImg from '../assets/heart-like-svgrepo-com.svg'
import NoData from "./NoData";
export type MatchCardInterface =  {
    id: string
    name: string,
    imageData: string,
    imageFormat: string,
    isVaccinated: boolean,
    description: string,
    isSterilized: boolean,
    age: number,
    createdAt: string;
}
export default function TabSelection() {
    const [matchCards, setMatchCards] = useState<MatchCardInterface[]>([])
    const [activeTab, setActiveTab] = useState(0);
    const [loading, setLoading] = useState(false);
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        setLoading(true);
        getMatches()
        .then((data) => {
            const transformedCards = data.map(card => {
                return {
                    id: card.matchId,
                    name: card.profileName,
                    imageData : card.images[0].imageData,
                    imageFormat: card.images[0].imageFormat,
                    isVaccinated: card.isVaccinated,
                    description: card.description,
                    isSterilized: card.isSterilized,
                    age: card.age,
                    createdAt: card.createdAt
                }
            })

            setMatchCards(transformedCards);
        })
        .catch(error => {
            console.error(error)
        })
        .finally(() => {
            setLoading(false);
        });
    }, [])
    const handleTabChange = (event : React.SyntheticEvent, value : number) => {
        setActiveTab(value);
    };

    const handleMatchSelect = (id : string) => {
        console.log("match card selected");
    }
    return(
        <>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Tabs 
                    value={activeTab} 
                    onChange={handleTabChange} 
                    aria-label="basic tabs example" 
                    sx={{
                        '& .MuiTabs-indicator': {
                            display: 'flex',
                            justifyContent: 'center',
                            backgroundColor: '#FEFAF6',
                        }}} >
                    <Tab
                        sx={{
                            color:"#000",
                            '&.Mui-selected': {
                                color: '#FEFAF6'
                            }
                        }} 
                        label="Matches"/>
                    <Tab 
                        sx={{
                            color:"#000", 
                            '&.Mui-selected': {
                                color: '#FEFAF6'
                            }
                        }} 
                        label="Messages"/>
                </Tabs>
            </Box>

            <Box sx={{ p: 2 }}>
                {activeTab === 0 && (
                    loading ? (
                    <CircularProgress sx={{ color: "#864c4c", marginTop:'50px' }} size={48} />
                    ) : (
                    <div>
                        {matchCards.length > 0 ? (
                        <div className="matches">
                            {matchCards.map((card) => (
                            <MatchCard
                                key={card.id}
                                {...card}
                                cards={matchCards}
                                setCards={setMatchCards}
                                handleMatchSelect={handleMatchSelect}
                            />
                            ))}
                        </div>
                        ) : <NoData imageSrc={matchImg} title="No Matches yet!" description="When you match with other users, the will appear here and you can navigate to their profile!"/>}
                    </div>
                    )
                )}
                {activeTab === 1 && (
                    <NoData imageSrc={messagesImg} title="Say Hello!" description="Looking for a strike up conversation? When you match with others, you can send them messages and communicate!"/>
                )}
            </Box>
        </>
    );
}

