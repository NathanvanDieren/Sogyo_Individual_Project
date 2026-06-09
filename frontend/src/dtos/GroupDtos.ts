export interface GroupListDto {
    groups: GroupDto[]
    totalCount: number
}

export interface GroupDto {
    id: string
    name: string
    members:  GroupMemberDto[]
}

export interface GroupMemberDto {
    id: string
    name: string
    email: string
}