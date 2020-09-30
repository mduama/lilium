import React from "react";

export interface ICrewMemberProps {
    id: number,
    name: string,
    base: string,
    scheduledFlight: object[],
    onSchedule: (id: number) => void
}

export const CrewMember = (props: ICrewMemberProps): JSX.Element => {
    return (
        <tr>
            <td>{props.id}</td>
            <td><p className="font-weight-bold">{props.name}</p></td>
            <td>{props.base}</td>
            <td>{props.scheduledFlight ? props.scheduledFlight.length : 0}</td>
            <td><button type="submit" className="btn btn-outline-primary" onClick={() => props.onSchedule(props.id)}>Schedule Flight</button></td>
        </tr>
    )
}
