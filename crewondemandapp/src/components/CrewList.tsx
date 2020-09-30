import React from "react";
import { ICrewMemberProps, CrewMember } from "./CrewMember";

export interface ICrewListProps {
    crewList: ICrewMemberProps[],
    onSchedule: (id: number) => void
}

export const CrewList = (props: ICrewListProps): JSX.Element => {
    return props.crewList ? (
        <table className="table table-sm">
            <thead>
                <tr>
                    <th scope="col">Pilot Id</th>
                    <th scope="col">Name</th>
                    <th scope="col">Location</th>
                    <th scope="col">Fligths Scheduled</th>
                    <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                {renderCrewList(props)}
            </tbody>
        </table>
    ) : <></>;
}

const renderCrewList = (props: ICrewListProps): JSX.Element[] => {
    return props.crewList.map((item: ICrewMemberProps) => {
        return <CrewMember key={item.id} id={item.id} name={item.name} base={item.base} scheduledFlight={item.scheduledFlight} onSchedule={(id: number) => {props.onSchedule(item.id)}}></CrewMember>
    });
}
